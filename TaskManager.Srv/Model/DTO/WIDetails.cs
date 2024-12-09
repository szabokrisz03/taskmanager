using Newtonsoft.Json;

namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class WIDetails
{
    public int Count { get; set; }
    public List<WiData> Value { get; set; } = new List<WiData>(0);
}

[Serializable]
public class WiData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("rev")]
    public int Rev { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; } = "";

    [JsonProperty("fields")]
    public WorkItem Fields { get; set; } = new();
}