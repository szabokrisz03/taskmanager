using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using MudBlazor;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.MilestoneServices;

namespace TaskManager.Srv.Components.TaskDetails;

/// <summary>
/// Határidő létrehozása.
/// </summary>
public partial class Milestone
{
    private List<MilestoneViewModel> milestoneList = new();
    private MudTable<MilestoneViewModel>? milestoneTable;
    private MilestoneViewModel? milestoneBeforeEdit;
    private Snackbar? _snackbar;
    [Parameter] public bool IsOpen { get; set; }
    [CascadingParameter(Name = "TaskId")] private long Id { get; set; }
    [Inject] public IMilestoneService? milestoneService { get; set; } = null;
    [Inject] public IMilestoneViewService? MilestoneViewService { get; set; } = null;
    [Inject] private IDialogService? DialogService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        milestoneList = await milestoneService!.ListMilestones(Id);
    }

    /// <summary>
    /// Szerkesztés esetén egy "backup"-ot készít a határidőről
    /// </summary>
    /// <param name="modell">A szerkesztett határidő viewmodelje</param>
    private void BackUpItem(object modell)
    {
        milestoneBeforeEdit = new()
        {
            Name = ((MilestoneViewModel)modell).Name,
            Table = ((MilestoneViewModel)modell).Table,
            RowId = ((MilestoneViewModel)modell).RowId,
            TaskId = ((MilestoneViewModel)modell).TaskId,
            Planned = ((MilestoneViewModel)modell).Planned,
            Actual = ((MilestoneViewModel)modell).Actual,
        };
    }

    /// <summary>
    /// Szerkesztés visszavonása
    /// </summary>
    /// <param name="modell">A szerkesztett határidő viewmodelje</param>
    private void ResetMilestoneToOriginal(object modell)
    {
        ((MilestoneViewModel)modell).Name = milestoneBeforeEdit!.Name;
        ((MilestoneViewModel)modell).Planned = milestoneBeforeEdit!.Planned;
    }

    /// <summary>
    /// Határidő szerkesztése
    /// </summary>
    /// <param name="modell">A szerkesztendő határidő viewmodelje</param>
    private void UpdateMilestone(object modell)
    {
        if (((MilestoneViewModel)modell).Planned < DateTime.Now)
        {
            _snackbar = Snackbar.Add("Nem állíthatsz be régebbi dátumot!", Severity.Warning, configure =>
            {
                configure.VisibleStateDuration = 3000;
                configure.HideTransitionDuration = 200;
                configure.ShowTransitionDuration = 200;
                configure.ShowCloseIcon = true;
            });
            milestoneTable!.ReloadServerData();
            return;
        }

        try
        {
            milestoneService!.UpdateMilestonekDbSync((MilestoneViewModel)modell);
            _snackbar = Snackbar.Add("Sikeres módosítás!", Severity.Success, configure =>
            {
                configure.VisibleStateDuration = 3000;
                configure.HideTransitionDuration = 200;
                configure.ShowTransitionDuration = 200;
                configure.ShowCloseIcon = true;
            });
            milestoneTable!.ReloadServerData();
        }
        catch (DbUpdateException)
        {
            _snackbar = Snackbar.Add("A megadott névvel már van határidő megadva!", Severity.Warning, configure =>
            {
                configure.VisibleStateDuration = 3000;
                configure.HideTransitionDuration = 200;
                configure.ShowTransitionDuration = 200;
                configure.ShowCloseIcon = true;
            });

            milestoneTable!.ReloadServerData();
        }
    }

    /// <summary>
    /// Határidő létrehozása.
    /// </summary>
    private async Task CreateMilestone()
    {
        await MilestoneViewService!.CreateMilestoneDialog(Id);
        await ListMilestones();
        if (!IsOpen)
        {
            await ToggleDrawer();
        }

        if (milestoneTable != null)
        {
            await milestoneTable.ReloadServerData();
        }
    }

    /// <summary>
    /// Határidők listázása.
    /// </summary>
    private async Task ListMilestones()
    {
        milestoneList = await milestoneService!.ListMilestones(Id);
    }

    private async Task<TableData<MilestoneViewModel>> LoadData(TableState state)
    {
        int size = await milestoneService!.CountTaskMilestone(Id);
        var milestones = await milestoneService.ListMilestones(Id);

        return new TableData<MilestoneViewModel>
        {
            Items = milestones,
            TotalItems = size
        };

    }

    /// <summary>
    /// Határidő lezárásához felugró ablak.
    /// </summary>
    /// <param name="milestoneView">Határidő</param>
    private async Task PopUpButton(MilestoneViewModel milestoneView)
    {
        if (milestoneView.Actual == null)
        {
            bool? result = await DialogService!.ShowMessageBox(
            "Határidő lezárása", (MarkupString)$"Biztos lezárod a határidőt? <br /> A határidő lezárása nem vonható vissza!",
            yesText: "Lezárás", cancelText: "Mégse"
            );

            if (result != null)
            {
                await CloseMilestone(milestoneView.RowId);
                await ListMilestones();
                await milestoneTable!.ReloadServerData();
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// Határidő törléséhez felugró ablak.
    /// </summary>
    /// <param name="milestone">Határidő</param>
    private async Task DeletePopUpButton(MilestoneViewModel milestone)
    {
        bool? result = await DialogService!.ShowMessageBox(
        "Határidő törlése", (MarkupString)$"Biztos törlöd a határidőt? <br /> A határidő törlése nem vonható vissza!",
        yesText: "Törlés", cancelText: "Mégse"
        );

        if (result != null)
        {
            await Delete(milestone.RowId);
            await ListMilestones();
            await milestoneTable!.ReloadServerData();
        }

        StateHasChanged();
    }

    /// <summary>
    /// Határidő törlése.
    /// </summary>
    /// <param name="milestoneId">Törlendő határidő egyedi azonosítója</param>
    /// <returns></returns>
    private async Task Delete(long milestoneId)
    {
        await milestoneService!.DeleteMilestone(milestoneId);
    }

    /// <summary>
    /// Határidő lezárása (ha elkészült).
    /// </summary>
    /// <param name="milestoneId">Lezárandó határidő egyedi azonosítója</param>
    private async Task CloseMilestone(long milestoneId)
    {
        await milestoneService!.CloseMilestone(milestoneId);
    }

    /// <summary>
    /// Lenyíló menüt kezeli.
    /// </summary>
    private async Task ToggleDrawer()
    {
        await ListMilestones();
        IsOpen = !IsOpen;

    }
}
