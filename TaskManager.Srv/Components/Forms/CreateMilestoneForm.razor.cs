using FluentValidation;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Collections.Immutable;

using TaskManager.Srv.Model.Validation;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Components.Forms;

public partial class CreateMilestoneForm
{
    private string[] _errors = new string[0];
    private ImmutableArray<string> Errors => _errors.ToImmutableArray();
    [Parameter] public MilestoneViewModel milestoneView { get; set; } = new();
    [Parameter] public EventCallback<bool> OnValidate { get; set; }
    [Inject] public MilestoneValidator Validator { get; private set; } = null!;

    /// <summary>
    /// Validálás.
    /// </summary>
    public Func<object, string, Task<IEnumerable<string>>> FieldValidator => async (model, field) =>
    {
        var result = await Validator.ValidateAsync(ValidationContext<MilestoneViewModel>.CreateWithOptions((MilestoneViewModel)model, x => x.IncludeProperties(field).IncludeProperties(nameof(MilestoneViewModel.TaskId))));
        await OnValidate.InvokeAsync(result.IsValid);
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage).ToArray();
    };
}
