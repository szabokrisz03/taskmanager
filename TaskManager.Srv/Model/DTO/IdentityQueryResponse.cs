using Newtonsoft.Json;

namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class IdentityQueryResponse
{
    [JsonProperty("results")]
    public List<IdentityQueryWrapperDTO> Results { get; set; } = new();
}

[Serializable]
public sealed class IdentityQueryWrapperDTO
{
    [JsonProperty("queryToken")]
    public string QueryToken { get; set; } = "";

    [JsonProperty("identities")]
    public List<IdentityQueryResponseFieldsDTO> Identities { get; set; } = new();
}

[Serializable]
public sealed class IdentityQueryResponseFieldsDTO
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; } = "";

    [JsonProperty("scopeName")]
    public string Domain { get; set; } = "";

    [JsonProperty("signInAddress")]
    public string UserName { get; set; } = "";
}