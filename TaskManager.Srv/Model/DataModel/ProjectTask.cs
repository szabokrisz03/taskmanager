namespace TaskManager.Srv.Model.DataModel;

public class ProjectTask : DbTable
{
    public long ProjectId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid TechnicalName { get; set; }
    public int Priority { get; set; }
    public TaskState State { get; set; }
    public Project? Project { get; set; }
    public string? ResponsiblePerson { get; set; }
    public ICollection<TaskMilestone>? Milestones { get; set; }
}