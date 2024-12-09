using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Components.Dialogs;

using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.MilestoneServices;

public class MilestoneViewService : IMilestoneViewService
{
    private readonly IDialogService dialogService;
    private readonly IMilestoneService milestoneService;
    [CascadingParameter] private MudDialogInstance Dialog { get; set; } = null!;

    public MilestoneViewService(IDialogService dialogService, IMilestoneService milestoneService)
    {
        this.dialogService = dialogService;
        this.milestoneService = milestoneService;
    }

    /// <inheritdoc cref="IMilestoneViewService.CreateMilestoneDialog(long))"/>
    public async Task CreateMilestoneDialog(long id)
    {
        var parameters = new DialogParameters
        {
            ["TaskId"] = id,
        };

        var dialog = await dialogService.ShowAsync<CreateMilestoneDialog>("Új mérföldkő", parameters);
        var result = await dialog.Result;

        if (result.Data != null)
        {
            MilestoneViewModel milestoneViewModel = (MilestoneViewModel)result.Data;

            await milestoneService.CreateMilestone(milestoneViewModel);
        }
    }
}