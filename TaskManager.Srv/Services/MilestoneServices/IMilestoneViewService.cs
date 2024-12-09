namespace TaskManager.Srv.Services.MilestoneServices;

public interface IMilestoneViewService
{
    /// <summary>
    /// Dialógus elkészítése határidő létrehozásánál.
    /// </summary>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    /// <returns></returns>
    Task CreateMilestoneDialog(long id);
}