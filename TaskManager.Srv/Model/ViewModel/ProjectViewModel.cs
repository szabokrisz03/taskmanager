using System.ComponentModel.DataAnnotations;

namespace TaskManager.Srv.Model.ViewModel;

/// <summary>
/// Projektet ábrázoló modell
/// </summary>
public class ProjectViewModel
{
    public long RowId { get; set; }

    [Display(Name = "Név")]
    public string Name { get; set; } = "";

    public Guid TechnicalName { get; set; }

    public DateTime LastVisit { get; set; }
}