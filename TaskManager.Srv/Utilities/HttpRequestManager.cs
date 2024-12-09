namespace TaskManager.Srv.Utilities;

public static class HttpRequestManager
{
    public static async Task<T?> GetAsync<T>(this HttpClient client, HttpRequestData request, string? additionalBase = null)
    {
        RebaseBaseChecked(client, additionalBase);

        using (var response = await client.GetAsync(request.Url))
        {
            return await response.Read<T>();
        }
    }

    private static void RebaseBaseChecked(HttpClient client, string? additionalBase)
    {
        if (additionalBase != null && client.BaseAddress != null)
        {
            RebaseBase(client, additionalBase);
        }
    }

    private static void RebaseBase(HttpClient client, string additionalBase)
    {
        if (additionalBase[0] == '/')
        {
            additionalBase = additionalBase[1..];
        }

        if (!additionalBase.EndsWith("/"))
        {
            additionalBase += "/";
        }

        client.BaseAddress = new Uri(client.BaseAddress!.AbsolutePath.Replace("/_apis/", "/") + additionalBase + "_apis");
    }
}
