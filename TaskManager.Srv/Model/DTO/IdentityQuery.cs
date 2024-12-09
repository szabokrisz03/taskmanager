using Newtonsoft.Json;

namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class IdentityQuery
{
    [JsonProperty("query")]
    public string Query { get; set; } = "";

    [JsonProperty("identityTypes")]
    public string[] IdentityTypes { get; set; } = new string[0];

    [JsonProperty("operationScopes")]
    public string[] OperationScopes { get; set; } = new string[0];

    [JsonProperty("properties")]
    public string[] Properties { get; set; } = new string[0];

    [JsonProperty("filterByEntityIds")]
    public string[] FilterByEntityIds { get; set; } = new string[0];

    [JsonProperty("options")]
    public IdentityQueryOptionsDTO Options { get; set; } = new();
}

[Serializable]
public sealed class IdentityQueryOptionsDTO
{
    public int MinResults { get; set; } = 10;
    public int MaxResults { get; set; } = 10;
}