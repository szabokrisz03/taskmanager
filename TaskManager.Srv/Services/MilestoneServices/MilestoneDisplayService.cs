using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;

namespace TaskManager.Srv.Services.MilestoneServices;

public class MilestoneDisplayService : IMilestoneDisplayService
{
    private readonly IDbContextFactory<ManagerContext> dbContextFactory;

    public MilestoneDisplayService(IDbContextFactory<ManagerContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc cref="IMilestoneDisplayService.MilestoneNameExistsAsync(long, string)"/>
    public async Task<bool> MilestoneNameExistsAsync(long taskId, string name)
    {
        using (var dbcx = dbContextFactory.CreateDbContext())
        {
            return await dbcx.TaskMilestone
                .AsNoTracking()
                .Where(p => p.TaskId == taskId && p.Name == name)
                .AnyAsync();
        }
    }
}