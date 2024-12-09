using Newtonsoft.Json;

using System.Text;
using System.Web;

namespace TaskManager.Srv.Utilities;

public class HttpRequestBuilder
{
    private readonly List<string> paramOrder = new();
    private readonly Dictionary<string, string> paramValues = new();
    private readonly string url;
    private string? bodyJson = null;

    public HttpRequestBuilder(string url, string apiVer = "6.0-preview.3")
    {
        this.url = url;
        SetQueryParam("api-version", apiVer);
    }

    public HttpRequestBuilder SetQueryParam(string name, string value)
    {
        if (paramValues.ContainsKey(name))
        {
            paramValues[name] = value;
        }
        else
        {
            paramOrder.Add(name);
            paramValues.Add(name, value);
        }

        return this;
    }

    public string GetQueryParam(string name) => paramValues[name];

    public bool TryGetQueryParam(string name, out string? value)
    {
        value = null;
        return paramValues.TryGetValue(name, out value);
    }

    public HttpRequestBuilder SetBody<T>(T payload)
    {
        bodyJson = JsonConvert.SerializeObject(payload);
        return this;
    }

    public string GetQuery()
    {
        bool firstParam = true;
        var sb = new StringBuilder();
        sb.Append(url);

        foreach (var name in paramOrder)
        {
            if (firstParam)
            {
                firstParam = false;
                sb.Append('?');
            }
            else
            {
                sb.Append('&');
            }

            sb.Append(HttpUtility.UrlEncode(name));
            sb.Append('=');
            sb.Append(HttpUtility.UrlEncode(paramValues[name]));
        }

        return sb.ToString();
    }

    public HttpContent GetContent()
    {
        var content = new StringContent(bodyJson!, Encoding.UTF8, "application/json");
        return content;
    }

    public HttpRequestData Build()
    {
        HttpRequestData request;
        HttpContent? content = null;

        if (bodyJson != null)
        {
            content = GetContent();
        }

        string query = GetQuery();
        request = new HttpRequestData(query, content);
        return request;
    }
}
