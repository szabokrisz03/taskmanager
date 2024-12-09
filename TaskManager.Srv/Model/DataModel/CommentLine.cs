namespace TaskManager.Srv.Model.DataModel;

public class CommentLine : DbTable
{
    public long TaskId { get; set; }
    public string Comment { get; set; } = "";
    public long UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public ProjectTask? ProjectTask { get; set; }
}