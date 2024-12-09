namespace TaskManager.Srv.Services.TaskServices;

/// <summary>
/// Message-ek és formok megjelenítésére
/// </summary>
public interface ITaskViewService
{
    /// <summary>
    /// Feladat dialógusát hozza létre.
    /// </summary>
    /// <param name="TechnicalName">Projekt technikai neve</param>
    Task CreateTaskDialog(string TechnicalName);
}
