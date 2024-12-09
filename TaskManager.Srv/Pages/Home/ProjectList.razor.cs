using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

using MudBlazor;

using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;

namespace TaskManager.Srv.Pages.Home;

public partial class ProjectList
{
    private string _userName = "";
    private List<ProjectViewModel> _projects = new();
    [Parameter] public Task<long> _projectId { get; set; } = null!;
    [Parameter] public bool MineOnly { get; set; } = false;
    [Parameter] public string SearchTerm { get; set; } = "";
    [CascadingParameter(Name = "projId")] private long projectId { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;
    [Inject] private IProjectDisplayService _projectDisplayService { get; set; } = null!;
    [Inject] private IProjectAdminService _projectAdminService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (authenticationStateTask != null)
        {
            var state = await authenticationStateTask;
            _userName = state?.User.Identity?.Name ?? "";
        }
    }

    public async Task AssignUserToProject()
    {
        await _projectAdminService.AssignProjectUserAsync(projectId, _userName);
    }

    protected override async Task OnParametersSetAsync()
    {
        await RefreshProjects();
    }

    /// <summary>
	/// Projektek frissítése.
	/// </summary>
	/// <param name="progress">progress</param>
    public async Task RefreshProjects(bool progress = false)
    {
        int skip = 0;
        if (progress)
        {
            skip = _projects.Count;
        }

        _projects = MineOnly
            ? await _projectDisplayService.ListUserProjectsAsync(_userName, SearchTerm, 100, skip)
            : await _projectDisplayService.ListProjectsAsync(SearchTerm, 100, skip);

        StateHasChanged();
    }
}