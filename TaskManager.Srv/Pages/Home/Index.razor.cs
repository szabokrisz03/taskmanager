using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using TaskManager.Srv.Services.ProjectServices;

namespace TaskManager.Srv.Pages.Home;

public partial class Index
{
    private string _searchTerm { get; set; } = "";
    private ProjectList? projectList { get; set; }
    private string _userName = "";
    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;
    [Inject] private IProjectViewService projectViewService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (authenticationStateTask != null)
        {
            var authState = await authenticationStateTask;
            _userName = authState.User.Identity?.Name ?? "";
        }
    }

    /// <summary>
	/// Új projekt hozzáadása.
	/// </summary>
    private async Task AddNewProject()
    {
        await projectViewService.CreateProjectAsync(_userName);

        if (projectList != null)
            await projectList.RefreshProjects();
    }
}