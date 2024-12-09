using Newtonsoft.Json;

namespace TaskManager.Srv.Model.DTO;

[Serializable]
public class WorkItem : IComparable<WorkItem>
{
    [JsonProperty("System.Id")]
    public int Id { get; set; }

    [JsonProperty("System.State")]
    public string State { get; set; } = "";

    [JsonProperty("System.AssignedTo")]
    public AzdoUser? AssignedTo { get; set; }

    [JsonProperty("System.ChangedDate")]
    public DateTime ChangedDate { get; set; }

    [JsonProperty("System.CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [JsonProperty("System.Title")]
    public string Title { get; set; } = "";

    [JsonProperty("System.WorkItemType")]
    public string Type { get; set; } = "";

    [JsonProperty("System.Url")]
    public string Url { get; set; } = "";

    [JsonProperty("System.TeamProject")]
    public string TeamProject { get; set; } = "";

    [JsonIgnore]
    public string? AssignedToId => AssignedTo?.UniqueName;

    public bool IsOpen { get; set; } = false;
    public string ClearState { get; set; } = "";
    public string AzdoLink { get; set; } = "";

    public int CompareTo(WorkItem? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var orderByType = new Dictionary<string, int>()
        {
            {"Rendszerszervezési feladat", 0 },
            {"Fejlesztési feladat", 1 },
            {"Technikai változásjelentés", 2 },
            {"Hibajegy", 3 },
            {"Egyéb feladat", 4 },
        };

        var orderByState = new Dictionary<string, int>()
        {
            {"In Progress", 0 },
            {"To Do", 1 },
            {"New", 1},
            {"Kiadásra vár", 2 },
            {"Fejlesztés kész", 3},
            {"Done", 4 },
            {"Rejected", 4 },
            {"Removed", 4 },
        };

        int typeStatus = orderByType[Type].CompareTo(orderByType[other.Type]);

        if (typeStatus != 0)
        {
            return typeStatus;
        }

        int stateStatus = orderByState[State].CompareTo(orderByState[other.State]);

        return stateStatus != 0 ? stateStatus : CreatedDate.CompareTo(other.CreatedDate);
    }
}