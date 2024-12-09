namespace TaskManager.Srv.Services.TaskServices;

/// <summary>
/// Feladatok neveinek összehasonlítására.
/// </summary>
public interface ITaskDisplayService
{
    /// <summary>
    /// Megnézi, hogy a megadott projektben létezik-e már az adott nevű feladat.
    /// </summary>
    /// <param name="projectId">Projekt egyedi azonosítója</param>
    /// <param name="name">A feladat neve</param>
    /// <returns>True, ha létezik, false egyébként</returns>
    Task<bool> TaskNameExistsAsync(long projectId, string name);
}
