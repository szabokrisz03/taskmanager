using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.DTO;

namespace TaskManager.Srv.Model.ViewModel;

/// <summary>
/// Feladatot ábrázoló modell
/// </summary>
public class TaskViewModel : IComparable<TaskViewModel>
{
    public long ProjectId { get; set; }
    public long RowId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int Priority { get; set; }
    public TaskState State { get; set; }
    public string? ResponsiblePerson { get; set; }

    public int CompareTo(TaskViewModel? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var sortByState = new Dictionary<TaskState, int>()
        {
            {TaskState.Ajanlatadas, 0},
            {TaskState.Igeny_felmeres, 0},
            {TaskState.Specifikacio_alatt, 0},
            {TaskState.Fejlesztesre_var, 0},
            {TaskState.Fejlesztes_alatt, 0},
            {TaskState.Teszteles_alatt, 0},
            {TaskState.Kiadasra_var, 0},
            {TaskState.Verziozva, 1},
            {TaskState.Meghiusult, 2},
        };

        int status = sortByState[State].CompareTo(sortByState[other.State]);
        return status != 0 ? status : Priority.CompareTo(other.Priority);
    }
}