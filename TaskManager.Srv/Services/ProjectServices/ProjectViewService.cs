using MudBlazor;

using TaskManager.Srv.Components.Dialogs;

namespace TaskManager.Srv.Services.ProjectServices;

public class ProjectViewService : IProjectViewService
{
    private readonly IDialogService dialogService;

    public ProjectViewService(
        IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    /// <inheritdoc cref="IProjectViewService.CreateProjectAsync(string)"/>
    public async Task CreateProjectAsync(string userName = "")
    {
        var parameters = new DialogParameters
        {
            ["UserName"] = userName
        };

        var dialog = await dialogService.ShowAsync<CreateProjectDialog>("Projekt létrehozása", parameters);
        await dialog.Result;
    }
}
