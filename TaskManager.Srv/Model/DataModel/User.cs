namespace TaskManager.Srv.Model.DataModel;

public class User : DbTable
{
    public string UserName { get; set; } = "";
    public string FullName { get; set; } = "";
    public ICollection<ProjectUser>? ProjectUsers { get; set; }
}