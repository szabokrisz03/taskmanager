using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;

namespace TaskManager.Srv.Services.TaskServices;

public class TaskDisplayService : ITaskDisplayService
{
    private readonly IDbContextFactory<ManagerContext> dbContextFactory;

    public TaskDisplayService(IDbContextFactory<ManagerContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc cref="ITaskDisplayService.TaskNameExistsAsync(long, string)"/>
    public async Task<bool> TaskNameExistsAsync(long projectId, string name)
    {
        using (var dbcx = dbContextFactory.CreateDbContext())
        {
            return await dbcx.ProjectTask
                .AsNoTracking()
                .Where(p => p.ProjectId == projectId && p.Name == name)
                .AnyAsync();
        }
    }
}