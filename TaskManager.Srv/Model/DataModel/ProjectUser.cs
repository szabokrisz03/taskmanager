using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Srv.Model.DataModel;

public class ProjectUser : IDbTable
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RowId { get; set; }
    public long ProjectId { get; set; }
    public long UserId { get; set; }
    public Project? Project { get; set; }
    public User? User { get; set; }
    public DateTime LastVisit { get; set; }
}