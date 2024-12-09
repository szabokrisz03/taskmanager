namespace TaskManager.Srv.Model.DataModel;

public class Discussion : DbTable
{
    public long ProjectId { get; set; }
    public long TaskId { get; set; }
    public ICollection<CommentLine>? CommentLines { get; set; }
}