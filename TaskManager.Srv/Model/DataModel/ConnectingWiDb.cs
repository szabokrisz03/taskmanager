namespace TaskManager.Srv.Model.DataModel;

public class ConnectingWiDb : DbTable
{
    public int WiId { get; set; }
    public long TaskId { get; set; }
}