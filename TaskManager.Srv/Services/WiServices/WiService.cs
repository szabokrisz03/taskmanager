using Microsoft.EntityFrameworkCore;

using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.DataModel;

namespace TaskManager.Srv.Services.WiServices;

public class WiService : IWiService
{

    private readonly IDbContextFactory<ManagerContext> dbContextFactory;

    public WiService(IDbContextFactory<ManagerContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc cref="IWiService.DeleteWi(int)"/>
    public async Task DeleteWi(int id)
    {
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            await dbcx.ConnectingWiDb
                .Where(p => p.WiId == id)
                .ExecuteDeleteAsync();
        }
    }

    /// <inheritdoc cref="IWiService.CreateWiAsync(int, long)"/>
    public async Task CreateWiAsync(int wiId, long taskId)
    {
        ConnectingWiDb wi = new()
        {
            WiId = wiId,
            TaskId = taskId,
        };

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            await dbcx.ConnectingWiDb.AddAsync(wi);
            await dbcx.SaveChangesAsync();
            dbcx.Entry(wi).State = EntityState.Detached;
        }
    }

    /// <inheritdoc cref="IWiService.ListWorkItem(long)"/>
    public async Task<int[]> ListWorkItem(long taskId)
    {
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var lst = await dbcx.ConnectingWiDb
                .AsNoTracking()
                .Where(wi => wi.TaskId == taskId)
                .ToListAsync();

            return lst.Select(p => p.WiId).ToArray();
        }
    }
}
