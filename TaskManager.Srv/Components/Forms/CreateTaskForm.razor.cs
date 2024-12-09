using FluentValidation;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Collections.Immutable;

using TaskManager.Srv.Model.Validation;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Components.Forms;

public partial class CreateTaskForm
{
    private string[] _errors = new string[0];
    private ImmutableArray<string> Errors => _errors.ToImmutableArray();
    private MudChip? selectedChip = new() { Value = 5 };
    [Parameter] public long ProjectId { get; set; }
    [Parameter] public TaskViewModel taskViewModel { get; set; } = new();
    [Parameter] public EventCallback<bool> OnValidate { get; set; }
    [Inject] public TaskValidator Validator { get; private set; } = null!;

    /// <summary>
    /// Validálás.
    /// </summary>
    public Func<object, string, Task<IEnumerable<string>>> FieldValidator => async (model, field) =>
    {
        var result = await Validator.ValidateAsync(ValidationContext<TaskViewModel>.CreateWithOptions((TaskViewModel)model, x => x.IncludeProperties(field).IncludeProperties(nameof(TaskViewModel.ProjectId))));
        await OnValidate.InvokeAsync(result.IsValid);
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage).ToArray();
    };

    /// <summary>
    /// A lérehozandó feladat prioritását állítja be.
    /// </summary>
    /// <param name="idx">A prioritás, amit kiválasztottunk</param>
    public void checkedIcon(int idx)
    {
        taskViewModel.Priority = idx;
    }
}
