namespace TaskManager.Srv.Services.MilestoneServices;

public interface IMilestoneDisplayService
{
    /// <summary>
    /// Megvizsgálja, hogy a határidőnek adott név szerepel-e már az adatbázisban.
    /// </summary>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    /// <param name="name">Határidő neve</param>
    Task<bool> MilestoneNameExistsAsync(long taskId, string name);
}