using Newtonsoft.Json;

using System.Collections.Immutable;
using System.Text;

using TaskManager.Srv.Model.DTO;
using TaskManager.Srv.Utilities;
using TaskManager.Srv.Utilities.Exceptions;

namespace TaskManager.Srv.Services.WiServices;

public class WiStateService : IWiStateService
{
    private readonly IConfiguration configuration;

    public WiStateService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private const string ConnectingWiQuery =
        "SELECT [System.ID], [System.Title], [System.State], [System.IterationPath], [System.WorkItemType] " +
        "FROM workitemLinks " +
        "WHERE [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward' " +
        "AND [Source].[System.ID] IN ({0}) " +
        "AND [Target].[State] != 'Rejected' " +
        "AND [Target].[State] != 'Removed' " +
        "AND [Target].[State] != 'Teljesítés nélkül lezárva' " +
        "ORDER BY [Microsoft.VSTS.Common.Priority], [System.CreatedDate] DESC " +
        "MODE (Recursive) ";

    private static readonly ImmutableArray<string> WiFields = new string[]
    {
        "System.Title", "System.AssignedTo", "System.Id", "System.State", "System.ChangedDate", "System.WorkItemType", "System.CreatedDate", "System.TeamProject"
    }.ToImmutableArray();

    /// <inheritdoc cref="IWiStateService.ListConnectingWis(int)"/>
    public List<WorkItem> ListConnectingWis(int wiId)
    {
        HttpClient httpClient;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;

        using (httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
        {
            var config = GetConfig();
            var uri = ADOSUrls.GetUrl(config);

            string query = string.Format(ConnectingWiQuery, wiId);

            using (httpRequestMessage = new(HttpMethod.Post, uri))
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { Query = query }), Encoding.UTF8, "application/json");

                using (httpResponseMessage = httpClient.Send(httpRequestMessage))
                {
                    var content = httpResponseMessage.Content.ReadAsStringAsync();
                    var asd = content.Result;

                    string mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType?.ToLower() ?? "";
                    string encoding = httpResponseMessage.Content.Headers.ContentType?.CharSet?.ToLower() ?? "";

                    if (mediaType != "application/json")
                        throw new NonFatalException($"Hibás MediaType: {mediaType}! Várt: application/json");
                    if (encoding != "utf-8")
                        throw new NonFatalException($"Hibás MediaType: {mediaType}! Várt: utf-8");

                    WIQLResponse? responseDto;

                    try
                    {
                        using (var jsonData = httpResponseMessage.Content.ReadAsStream())
                            responseDto = ReadJSONResponse<WIQLResponse>(jsonData);
                    }
                    catch (Exception ex)
                    {
                        throw new NonFatalException("JSON exception: " + ex.Message, ex);
                    }

                    if (responseDto is null)
                    {
                        string errMsg = "A WI listázó REST kérés NULL értéket (JSON) adott vissza!";
                        throw new NonFatalException(errMsg);
                    }

                    var idArray = responseDto.workItemRelations.Select(t => t.target.id).ToArray();

                    return DetailWIs(idArray, idArray.Length);
                }
            }
        }
    }

    /// <inheritdoc cref="IWiStateService.queryMaker(IEnumerable{int})"/>
    public Dictionary<WorkItem, List<WorkItem>> queryMaker(IEnumerable<int> source)
    {

        Dictionary<WorkItem, List<WorkItem>> parentChildrenPairs = new();

        if (source == null)
        {
            return parentChildrenPairs;
        }

        foreach (var id in source)
        {
            var wiList = ListConnectingWis(id);

            var parentWi = wiList.Where(x => x.Id == id).FirstOrDefault();
            wiList.RemoveAll(x => x.Id == id);

            if (parentWi != null)
            {
                parentChildrenPairs.Add(parentWi, wiList);
            }
        }

        return parentChildrenPairs;
    }

    /// <inheritdoc cref="IWiStateService.DetailWIs(IEnumerable{int}, int)"/>
    public List<WorkItem> DetailWIs(IEnumerable<int> sources, int capacity = 0)
    {
        List<WorkItem> wilist = capacity <= 0 ? new() : new(capacity);
        int start = 0;
        IEnumerable<int> slice;

        while ((slice = sources.Take(new Range(start, start + 200))).Any())
        {
            start += 200;
            PropertyWis(slice, wilist);
        }

        return wilist;
    }

    /// <inheritdoc cref="IWiStateService.PropertyWis(IEnumerable{int}, List{WorkItem})"/>
    public void PropertyWis(IEnumerable<int> source, List<WorkItem> collector)
    {
        HttpClient httpClient;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;

        using (httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
        {
            var config = GetConfig();
            var uri = ADOSUrls.GetDetailUrl(config, source, WiFields);

            using (httpRequestMessage = new(HttpMethod.Get, uri))
            {
                using (httpResponseMessage = httpClient.Send(httpRequestMessage))
                {
                    string mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType?.ToLower() ?? "";
                    string encoding = httpResponseMessage.Content.Headers.ContentType?.CharSet?.ToLower() ?? "";

                    if (mediaType != "application/json")
                        throw new NonFatalException($"Hibás MediaType: {mediaType}! Várt: application/json");
                    if (encoding != "utf-8")
                        throw new NonFatalException($"Hibás MediaType: {mediaType}! Várt: utf-8");

                    WIDetails? responseDto;

                    try
                    {
                        using (var jsonData = httpResponseMessage.Content.ReadAsStream())
                            responseDto = ReadJSONResponse<WIDetails>(jsonData);
                    }
                    catch (Exception e)
                    {
                        throw new NonFatalException("JSON Exception: " + e.Message, e);
                    }

                    if (responseDto is null)
                    {
                        string errMsg = "A WI részletező REST kérés NULL értéket (JSON) adott vissza!";
                        throw new NonFatalException(errMsg);
                    }

                    collector.AddRange(responseDto.Value.Select(v =>
                    {
                        var wi = v.Fields;
                        wi.Id = v.Id;
                        wi.Url = v.Url;
                        wi.AzdoLink = UrlBuilder(wi.TeamProject, wi.Id);
                        return wi;
                    }));
                }
            }
        }
    }

    public string UrlBuilder(string teamProject, int id)
    {
        var config = GetConfig();
        return ADOSUrls.GetAzdoUrl(config, teamProject, id);
    }

    public static T? ReadJSONResponse<T>(Stream jsonData)
    where T : new()
    {
        JsonSerializer serializer;
        serializer = JsonSerializer.Create();

        using (var textReader = new StreamReader(jsonData, Encoding.UTF8, leaveOpen: true))
        using (var jsonReader = new JsonTextReader(textReader))
            return serializer.Deserialize<T>(jsonReader);
    }

    private ADOSConfig GetConfig()
    {
        ADOSConfig config = new();
        configuration.GetSection("AZDO").Bind(config);
        return config;
    }
}