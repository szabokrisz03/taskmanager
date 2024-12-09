namespace TaskManager.Srv.Model.DataModel;

public class TaskMilestone : DbTable
{
    public long TaskId { get; set; }
    public long MilestoneId { get; set; }
    public string Name { get; set; } = "";
    public DateTime Planned { get; set; }
    public DateTime? Actual { get; set; }
    public ProjectTask? Task { get; set; }
}