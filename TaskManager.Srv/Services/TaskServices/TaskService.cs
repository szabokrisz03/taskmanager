using AutoMapper;

using Microsoft.EntityFrameworkCore;

using MudBlazor;

using TaskManager.Srv.Model.DataContext;
using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<ManagerContext> dbContextFactory;
    private readonly Snackbar? _snackbar;

    public TaskService(IMapper mapper, IDbContextFactory<ManagerContext> dbContextFactory)
    {
        this.mapper = mapper;
        this.dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc cref="ITaskService.CountTasks(long)"/>
    public async Task<int> CountTasks(long projectId)
    {
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            return await dbcx.ProjectTask
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .CountAsync();
        }
    }

    /// <inheritdoc cref="ITaskService.ListTasksById(long, int, int)"/>
    public async Task<List<TaskViewModel>> ListTasksById(long projectId, int take, int skip = 0)
    {
        List<string> asd = new()
        {
            "Igeny_felmeres"
        };

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var lst = await dbcx.ProjectTask
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return lst.Select(mapper.Map<TaskViewModel>).Where(t => asd.Contains(t.State.ToString())).ToList();
        }
    }

    /// <inheritdoc cref="ITaskService.ListTasksByFilterAndId(List{string}, long, int, int)"/>
    public async Task<List<TaskViewModel>> ListTasksByFilterAndId(List<string> filterName, long projectId, int take, int skip = 0)
    {

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var lst = await dbcx.ProjectTask
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return lst.Select(mapper.Map<TaskViewModel>).Where(t => filterName.Contains(t.State.ToString())).ToList();
        }
    }

    /// <inheritdoc cref="ITaskService.UpdateTaskDb(TaskViewModel)"/>
    public async Task UpdateTaskDb(TaskViewModel modell)
    {
        await Task.Run(() => UpdateTaskDbSync(modell));
    }

    /// <inheritdoc cref="ITaskService.UpdateTaskDbSync(TaskViewModel)"/>
    public void UpdateTaskDbSync(TaskViewModel modell)
    {
        if (modell.RowId == 0)
        {
            return;
        }

        try
        {
            using (var dbcx = dbContextFactory.CreateDbContext())
            {
                var res = dbcx.ProjectTask.SingleOrDefault(x => x.RowId == modell.RowId);
                if (res == null)
                {
                    return;
                }

                var ent = mapper.Map<TaskViewModel, ProjectTask>(modell, res);
                dbcx.ProjectTask.Update(ent);
                dbcx.SaveChanges();
                dbcx.Entry(ent).State = EntityState.Detached;
            }
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    /// <inheritdoc cref="ITaskService.UpdateStatus(TaskViewModel)"/>
    public async Task UpdateStatus(TaskViewModel taskViewModel)
    {
        var task = mapper.Map<ProjectTask>(taskViewModel);

        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            var entry = dbcx.Attach(task);
            entry.Property(e => e.State).IsModified = true;
            await dbcx.SaveChangesAsync();

            entry.State = EntityState.Detached;
        }
    }

    /// <inheritdoc cref="ITaskService.CreateTask(TaskViewModel)"/>
    public async Task<TaskViewModel> CreateTask(TaskViewModel taskView)
    {
        var task = mapper.Map<ProjectTask>(taskView);
        using (var dbcx = await dbContextFactory.CreateDbContextAsync())
        {
            await dbcx.ProjectTask.AddAsync(task);
            await dbcx.SaveChangesAsync();
            dbcx.Entry(task).State = EntityState.Detached;
        }

        return mapper.Map<TaskViewModel>(task);
    }
}