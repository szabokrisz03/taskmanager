using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Components.Dialogs;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;

namespace TaskManager.Srv.Services.TaskServices;

public class TaskViewService : ITaskViewService
{
    private readonly IDialogService dialogService;
    private readonly ITaskService taskService;
    private readonly IProjectDisplayService projectDisplayService;

    [CascadingParameter] private MudDialogInstance Dialog { get; set; } = null!;

    public TaskViewService(IDialogService dialogService, ITaskService taskService, IProjectDisplayService projectDisplayService)
    {
        this.dialogService = dialogService;
        this.taskService = taskService;
        this.projectDisplayService = projectDisplayService;
    }

    /// <inheritdoc cref="ITaskViewService.CreateTaskDialog(string)"/>
    public async Task CreateTaskDialog(string technicalName)
    {
        Guid.TryParse(technicalName, out Guid TechnicalName);
        long projectId = await projectDisplayService.GetProjectIdAsync(TechnicalName);
        var parameters = new DialogParameters
        {
            ["ProjectId"] = projectId,
        };

        var dialog = await dialogService.ShowAsync<CreateTaskDialog>("Új Feladat", parameters);
        var result = await dialog.Result;

        if (result.Data != null)
        {
            TaskViewModel taskViewModel = (TaskViewModel)result.Data;

            await taskService.CreateTask(taskViewModel);
        }
    }
}