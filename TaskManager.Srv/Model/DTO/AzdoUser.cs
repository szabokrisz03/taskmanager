using Newtonsoft.Json;

namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class AzdoUser
{
    [JsonProperty("displayName")]
    public string? DisplayName { get; set; }

    [JsonProperty("uniqueName")]
    public string? UniqueName { get; set; }
}