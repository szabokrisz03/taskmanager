using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Components.Dialogs;

/// <summary>
/// Feladat dialógusa.
/// </summary>
public partial class CreateTaskDialog
{
    private TaskViewModel taskViewModel { get; set; } = new();
    private bool DisableSubmit = true;
    [Parameter] public string UserName { get; set; } = "";
    [Parameter] public long ProjectId { get; set; }
    [CascadingParameter] private MudDialogInstance Dialog { get; set; } = null!;

    private void OnValidate(bool isValid)
    {
        DisableSubmit = !isValid;
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        taskViewModel.ProjectId = ProjectId;
        base.OnParametersSet();
    }

    /// <summary>
    /// Feladat dialógus létrehozása.
    /// </summary>
    private void CreateTask()
    {
        if (taskViewModel.Priority == 0)
        {
            taskViewModel.Priority = 5;
        }

        Dialog.Close(DialogResult.Ok(taskViewModel));
    }

    /// <summary>
    /// Megszakítás.
    /// </summary>
    public void Cancel()
    {
        Dialog.Cancel();
    }
}
