using TaskManager.Srv.Model.DTO;

namespace TaskManager.Srv.Utilities;

/// <summary>
/// AZDO url-t összeépítő osztály a workitemek lekérdezéséhez.
/// </summary>
public class ADOSUrls
{
    private static string GetWiDetailsUrl(ADOSConfig config, IEnumerable<int> wis, IEnumerable<string> fields)
    => $"https://{config.Host}/{config.Collection}/{config.TeamProject}/_apis/wit/workitems?ids={string.Join(",", wis)}&api-version={config.ApiVer}&fields={string.Join(",", fields)}";

    private static string GetQueryUrl(ADOSConfig config)
    => $"https://{config.Host}/{config.Collection}/{config.TeamProject}/_apis/wit/wiql?api-version={config.ApiVer}";

    private static string GetAzdoUrls(ADOSConfig config, string teamProject, int id)
        => $"https://{config.Host}/{config.Collection}/{teamProject}/_workitems/edit/{id}";

    private static string GetAzdoUserUrls(ADOSConfig config)
        => $"https://{config.Host}/{config.Collection}/_apis/IdentityPicker/Identities?api-version=6.0-preview";

    /// <summary>
    /// A workitemek mezőinek a lekérdezéséhez szükséges URL összeállítása.
    /// </summary>
    /// <param name="config">Url összerakásához szükséges config</param>
    /// <param name="wis">Workitemeket tartalmazó tömb</param>
    /// <param name="fields">Workitemek mezőit tartalmazó tömb</param>
    /// <returns></returns>
    public static string GetDetailUrl(ADOSConfig config, IEnumerable<int> wis, IEnumerable<string> fields)
    {
        return GetWiDetailsUrl(config, wis, fields);
    }

    /// <summary>
    /// Workitem lekérdezéséhez szükséges URL összeállítása.
    /// </summary>
    /// <param name="config">Url összerakásához szükséges config</param>
    /// <returns></returns>
    public static string GetUrl(ADOSConfig config)
    {
        return GetQueryUrl(config);
    }

    /// <summary>
    /// Azure DevOpson az "ID"-hez tartozó workitem URL összerakása.
    /// </summary>
    /// <param name="config">Url összerakásához szükséges config</param>
    /// <param name="teamProject">Teamproject</param>
    /// <param name="id">Workitem ID</param>
    /// <returns></returns>
    public static string GetAzdoUrl(ADOSConfig config, string teamProject, int id)
    {
        return GetAzdoUrls(config, teamProject, id);
    }

    public static string GetAzdoUserUrl(ADOSConfig config)
    {
        return GetAzdoUserUrls(config);
    }
}
