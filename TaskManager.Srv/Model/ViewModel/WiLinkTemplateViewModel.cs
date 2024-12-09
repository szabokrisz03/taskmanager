using TaskManager.Srv.Model.DataModel;

namespace TaskManager.Srv.Model.ViewModel;

/// <summary>
/// WiLinkTemplate-t ábrázoló modell
/// </summary>
public class WiLinkTemplateViewModel
{
    public long RowId { get; set; }
    public long ProjectId { get; set; }
    public string Name { get; set; } = "";
    public WiType WiType { get; set; }
    public string TeamProject { get; set; } = "";
    public TargetIterationType Iteration { get; set; }
    public string? AssignedTo { get; set; }
}