using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.UtilityServices;

namespace TaskManager.Srv.Services.ProjectServices;

internal class ProjectAdminService : IProjectAdminService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<ManagerContext> dbContextFactory;
    private readonly IUserService userService;

    public ProjectAdminService(
        IMapper mapper,
        IDbContextFactory<ManagerContext> dbContextFactory,
        IUserService userService)
    {
        this.mapper = mapper;
        this.dbContextFactory = dbContextFactory;
        this.userService = userService;
    }

    /// <inheritdoc cref="IProjectAdminService.AssignProjectUserAsync(long, string)"/>
    public async Task<bool> AssignProjectUserAsync(long projectid, string userName)
    {
        var user = await userService.GetUser(userName);
        if (user is null)
        {
            return false;
        }

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var projectUser = await dbcx.ProjectUser.AsNoTracking().Where(pu => pu.ProjectId == projectid && pu.UserId == user.RowId).SingleOrDefaultAsync();
            if (projectUser != null)
            {
                return true;
            }

            projectUser = new ProjectUser()
            {
                ProjectId = projectid,
                UserId = user.RowId,
                LastVisit = DateTime.Now,
            };

            await dbcx.ProjectUser.AddAsync(projectUser);
            await dbcx.SaveChangesAsync();
            dbcx.Entry(projectUser).State = EntityState.Detached;

            return true;
        }
    }

    /// <inheritdoc cref="IProjectAdminService.CreateProjectAsync(ProjectViewModel)"/>
    public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel projectView)
    {
        var project = mapper.Map<Project>(projectView);
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            await dbcx.Project.AddAsync(project);
            await dbcx.SaveChangesAsync();
            dbcx.Entry(project).State = EntityState.Detached;
        }

        return mapper.Map<ProjectViewModel>(project);
    }

    /// <inheritdoc cref="IProjectAdminService.CreateProjectAsync(ProjectViewModel, string)"/>
    public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel projectView, string userName)
    {
        var projectVm = await CreateProjectAsync(projectView);
        await AssignProjectUserAsync(projectVm.RowId, userName);

        return projectVm;
    }
}
