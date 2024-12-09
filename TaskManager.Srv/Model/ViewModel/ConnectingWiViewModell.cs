namespace TaskManager.Srv.Model.ViewModel;

/// <summary>
/// ConnectingWi-t ábrázoló modell.
/// </summary>
public class ConnectingWiViewModell
{
    public long WiId { get; set; }
    public long TaskId { get; set; }
    public string Title { get; set; } = "";
    public string? AssignedTo { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime LastUpdate { get; set; }
}