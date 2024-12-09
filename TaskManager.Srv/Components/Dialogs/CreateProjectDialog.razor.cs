using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;
using TaskManager.Srv.Utilities.Exceptions;

namespace TaskManager.Srv.Components.Dialogs;

/// <summary>
/// Projekt dialógusa.
/// </summary>
public partial class CreateProjectDialog
{
    private ProjectViewModel Project { get; set; } = new();
    private bool DisableSubmit = true;
    [Parameter] public string UserName { get; set; } = "";
    [CascadingParameter] private MudDialogInstance Dialog { get; set; } = null!;
    [Inject] private IProjectAdminService ProjectAdminService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private void OnValidate(bool isValid)
    {
        DisableSubmit = !isValid;
        StateHasChanged();
    }

    /// <summary>
    /// Projekt dialógus létrehozása.
    /// </summary>
    private async Task CreateProject()
    {
        try
        {
            Project = await ProjectAdminService.CreateProjectAsync(Project, UserName);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

        if (Project.RowId != 0)
        {
            Dialog.Close(DialogResult.Ok(true));
        }
    }

    /// <summary>
    /// Megszakítás.
    /// </summary>
    public void Cancel()
    {
        Dialog.Cancel();
    }
}
