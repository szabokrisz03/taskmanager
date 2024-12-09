using FluentValidation;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;
using TaskManager.Srv.Services.TaskServices;
using Microsoft.EntityFrameworkCore;
using TaskManager.Srv.Services.AzdoServices;
using TaskManager.Srv.Model.DTO;

namespace TaskManager.Srv.Pages.Projects;

public partial class ProjectTasks
{
    private long ShownId = 0;
    private MudTable<TaskViewModel>? _table;
    private List<(TaskState value, string name)> enumList = new();
    private List<string>? filterNames;
    private Guid _lastTechnicalName;
    private long _projectId = 0;
    private TaskViewModel? taskBeforeEdit;
    private Snackbar? _snackbar;
    private string? responsibleUser;
    [Parameter] public string TechnicalName { get; set; } = "";
    [Parameter] public StateFilterViewModell? stateFilterView { get; set; }
    [Parameter] public TaskViewModel taskViewModel { get; set; } = new();
    [CascadingParameter] private long Id { get; set; }
    [CascadingParameter(Name = "stateFilterProps")] private StateFilterViewModell? stateFilter { get; set; }
    [Inject] private ITaskViewService TaskViewService { get; set; } = null!;
    [Inject] private ITaskService TaskService { get; set; } = null!;
    [Inject] private IProjectDisplayService project { get; set; } = null!;
    [Inject] private IAzdoUserService _azdoUserService { get; set; } = null!;

    protected override void OnParametersSet()
    {
        ListTaskState();
        stateFilterView = new();
    }

    protected override async Task OnParametersSetAsync()
    {
        Guid.TryParse(TechnicalName, out Guid technicalName);
        if (_lastTechnicalName != technicalName)
        {
            _lastTechnicalName = technicalName;
            _projectId = await project.GetProjectIdAsync(technicalName);
        }
    }

    private async Task<IEnumerable<string>> SearchUser(string value)
    {
        List<AzdoUser> azdoUsers = await _azdoUserService.SearchUsers(value);
        List<string> usernames = new();
        await Task.Delay(5);
        if(string.IsNullOrEmpty(value))
        {
            return new string[0];
        }

        foreach (var user in azdoUsers)
        {
            usernames.Add(user.DisplayName!);
        }

        if (usernames.Count == 0)
        {
            return new string[0];
        }

        return usernames;
    }

    private async Task UpdateUser(TaskViewModel taskViewModel)
    {
        await _azdoUserService.UpdateTaskUserDb(taskViewModel);
    }

    /// <summary>
    /// Módosítás visszavonásánál eltárolja a módosítatlan értékeket.
    /// </summary>
    /// <param name="model">A módosítandó feladat</param>
    private void BackUpItem(object model)
    {
        taskBeforeEdit = new()
        {
            Name = ((TaskViewModel)model).Name,
            Priority = ((TaskViewModel)model).Priority,
            RowId = ((TaskViewModel)model).RowId,
            ProjectId = ((TaskViewModel)model).ProjectId,
        };

    }

    /// <summary>
    /// Visszaállítja a BackUpItem által beállított értékeket
    /// </summary>
    /// <param name="modell">A módosítandó feladat</param>
    private void ResetTaskToOriginal(object modell)
    {
        ((TaskViewModel)modell).Name = taskBeforeEdit!.Name;
        ((TaskViewModel)modell).Priority = taskBeforeEdit.Priority;
    }

    /// <summary>
    /// Feladatok módosítását végzi el
    /// </summary>
    /// <param name="model">A módosítandó feladat</param>
    private void UpdateTask(object model)
    {
        try
        {
            TaskService.UpdateTaskDbSync((TaskViewModel)model);
            _snackbar = Snackbar.Add("Sikeres módosítás", MudBlazor.Severity.Success, configure =>
            {
                configure.VisibleStateDuration = 3000;
                configure.HideTransitionDuration = 200;
                configure.ShowTransitionDuration = 200;
                configure.ShowCloseIcon = true;
            });

            if (_table != null)
            {
                _table.ReloadServerData();
            }
        }
        catch(DbUpdateException)
        {
            _snackbar = Snackbar.Add("A megadott névvel már van létrehozva feladat!", MudBlazor.Severity.Warning, configure =>
            {
                configure.VisibleStateDuration = 3000;
                configure.HideTransitionDuration = 200;
                configure.ShowTransitionDuration = 200;
                configure.ShowCloseIcon = true;
            });

            if (_table != null)
            {
                _table.ReloadServerData();
            }
        }
    }

    /// <summary>
    /// Feladat létrehozása
    /// </summary>
    private async Task CreateTask()
    {
        await TaskViewService.CreateTaskDialog(TechnicalName);

        if (_table != null)
        {
           await _table.ReloadServerData();
        }
    }

    /// <summary>
    /// Táblázat feltöltése
    /// </summary>
    /// <param name="state">TableState</param>
    /// <returns>TableData<TaskViewModel></returns>
    private async Task<TableData<TaskViewModel>> LoadData(TableState state)
    {
        if (filterNames == null)
        {
            GetFilterData(stateFilterView!);
        }

        int skip = state.PageSize * state.Page;
        int size = await TaskService.CountTasks(_projectId);
        var tasks = await TaskService.ListTasksByFilterAndId(filterNames!, _projectId, size, skip);
        tasks.Sort();

        ShownId = 0;

        return new TableData<TaskViewModel>
        {
            Items = tasks,
            TotalItems = size
        };
    }

    /// <summary>
    /// Lekéri a filterek neveit a viewModelből.
    /// </summary>
    /// <param name="state">A filter viewModellje</param>
    private void GetFilterData(StateFilterViewModell state)
    {
        filterNames = new();

        foreach (var prop in stateFilterView!.GetType().GetProperties())
        {
            if ((bool)prop.GetValue(stateFilterView, null)!)
            {
                filterNames.Add(prop.Name);
            }
        }

        if (_table != null)
        {
            _table.ReloadServerData();
        }
    }

    /// <summary>
    /// Feladat státuszának frissítése.
    /// </summary>
    /// <param name="model">A frissítendő feladat</param>
    private async Task UpdateState(TaskViewModel model)
    {
        await TaskService.UpdateStatus(model);

        if (_table != null)
        {
            await _table.ReloadServerData();
        }
    }

    /// <summary>
    /// Feladat státuszainak beállítása.
    /// </summary>
    private void ListTaskState()
    {
        var type = typeof(TaskState);

        enumList = Enum.GetValues(typeof(TaskState))
            .Cast<TaskState>()
            .Select(v =>
            {
                string valueName = v.ToString();
                string name;
                name = type.GetMember(valueName).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? valueName;

                return (v, name);
            }).ToList();
    }

    /// <summary>
    /// A kiválasztott task háttérszínét változtatja meg.
    /// </summary>
    /// <param name="taskViewModel">A task viewmodellje</param>
    /// <param name="idx">Nem használjuk, a mudblazor keretrendszer kéri paraméterként</param>
    /// <returns></returns>
    private string TableRowStyle(TaskViewModel taskViewModel, int idx)
    {
        return ShownId == taskViewModel.RowId ? $"background-color: rgba(119, 107, 231, 0.3);" : "";
    }

    /// <summary>
    /// Lenyíló menüt szabályozza.
    /// </summary>
    /// <param name="taskView">A lenyíló feladat</param>
    private void ShowBtnPress(TaskViewModel taskView)
    {
        Id = taskView.RowId;
        ShownId = ShownId == taskView.RowId ? 0 : taskView.RowId;
    }
}