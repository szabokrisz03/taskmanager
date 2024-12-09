using AutoMapper;

using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Users;

using Newtonsoft.Json;

using TaskManager.Srv.Model.DTO;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Utilities;
using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.DataModel;

namespace TaskManager.Srv.Services.AzdoServices;

public class AzdoUserService : IAzdoUserService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;
    private HttpClient _httpClient;
    private readonly IDbContextFactory<ManagerContext> _dbContextFactory;

    public AzdoUserService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper, HttpClient httpClient, IDbContextFactory<ManagerContext> dbContextFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _httpClient = httpClient;
        _dbContextFactory = dbContextFactory;
    }

    public async Task UpdateTaskUserDb(TaskViewModel taskViewModel)
    {
        await Task.Run(() => UpdateTaskUserDbSync(taskViewModel));
    }

    public void UpdateTaskUserDbSync(TaskViewModel taskViewModel)
    {
        try
        {
            if (taskViewModel.RowId == 0)
            {
                return;
            }

            using (var dbcx = _dbContextFactory.CreateDbContext())
            {
                var res = dbcx.ProjectTask.SingleOrDefault(x => x.RowId == taskViewModel.RowId);
                if (res == null)
                {
                    return;
                }

                var ent = _mapper.Map<TaskViewModel, ProjectTask>(taskViewModel, res);
                dbcx.ProjectTask.Update(ent);
                dbcx.SaveChanges();
                dbcx.Entry(ent).State = EntityState.Detached;
            }
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<List<AzdoUser>> SearchUsers(string query)
    {
        var config = GetConfig();
        string uri = ADOSUrls.GetAzdoUserUrl(config);

        IdentityQueryResponse? responseDTO;

        if (string.IsNullOrEmpty(query))
        {
            return new List<AzdoUser>();
        }

        using (_httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
        using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri))
        {
            var queryObject = new IdentityQuery
            {
                Query = query,
                IdentityTypes = new string[] { "user" },
                OperationScopes = new string[] { "ims" },
                Properties = new string[]
                {
                    "DisplayName",
                    "ScopeName",
                    "SamAccountName",
                    "SignInAddress"
                },
                Options = new()
                {
                    MinResults = 10,
                    MaxResults = 10,
                }
            };

            httpRequest.Content = JsonContent.Create(queryObject);
            using (var response = await _httpClient.SendAsync(httpRequest))
            {
                ValidateResponse(response);
                using (var body = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(body))
                {
                    responseDTO = ReadJSONResponse<IdentityQueryResponse>(body);
                }
            }
        }

        if (responseDTO is null)
        {
            throw new UserException("Nem kaptam (helyes) választ a szervertől!");
        }

        var userList = responseDTO.Results.SelectMany(r => r.Identities).Select(i => new AzdoUser { DisplayName = i.DisplayName, UniqueName = $"{i.Domain}\\{i.UserName}" }).ToList();
        return responseDTO.Results.SelectMany(r => r.Identities).Select(i => new AzdoUser { DisplayName = i.DisplayName, UniqueName = $"{i.Domain}\\{i.UserName}" }).ToList();
    }

    private void ValidateResponse(HttpResponseMessage httpResponseMessage)
    {
        if (!httpResponseMessage.IsSuccessStatusCode)
            throw new Exception($"Failed HTTP response: {httpResponseMessage.StatusCode}");

        var contentType = httpResponseMessage.Content.Headers.ContentType;

        if (contentType?.CharSet?.ToLower() != "utf-8")
            throw new Exception($"Unexpected ({contentType?.CharSet}) encoding encountered! Expected UTF8!");

        if (contentType?.MediaType?.ToLower() != "application/json")
            throw new Exception($"Unexpected media type: {contentType?.MediaType}! Expected application/json");
    }

    private T? ReadJSONResponse<T>(Stream jsonData)
        where T : new()
    {
        JsonSerializer serializer;
        serializer = JsonSerializer.Create();

        using (var textReader = new StreamReader(jsonData, System.Text.Encoding.UTF8, leaveOpen: true))
        using (var jsonReader = new JsonTextReader(textReader))
            return serializer.Deserialize<T>(jsonReader);
    }

    private ADOSConfig GetConfig()
    {
        ADOSConfig config = new();
        _configuration.GetSection("AZDO").Bind(config);
        return config;
    }
}