namespace TaskManager.Srv.Utilities;

public class HttpRequestData
{
    public HttpRequestData(string url)
    {
        Url = url;
    }

    public HttpRequestData(string url, HttpContent content)
    {
        Url = url;
        Content = content;
    }

    public string Url { get; init; }
    public HttpContent? Content { get; init; }
}
