using TaskManager.Srv.Model.DTO;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.AzdoServices;

public interface IAzdoUserService
{
    Task<List<AzdoUser>> SearchUsers(string query);
    void UpdateTaskUserDbSync(TaskViewModel taskViewModel);
    Task UpdateTaskUserDb(TaskViewModel taskViewModel);
}
