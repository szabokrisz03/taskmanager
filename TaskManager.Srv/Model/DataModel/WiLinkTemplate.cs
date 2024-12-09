namespace TaskManager.Srv.Model.DataModel;

[Serializable]
public class WiLinkTemplate : DbTable
{
    public long ProjectId { get; set; }
    public string Name { get; set; } = "";
    public WiType WiType { get; set; }
    public string TeamProject { get; set; } = "";
    public TargetIterationType Iteration { get; set; }
    public string? AssignedTo { get; set; }
    public Project? Project { get; set; }
}