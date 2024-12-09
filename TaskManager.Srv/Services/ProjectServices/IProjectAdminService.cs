using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.ProjectServices;

/// <summary>
/// Projekt létrehozására és felhasználó hozzáarendelésére.
/// </summary>
public interface IProjectAdminService
{
    /// <summary>
    /// Projekt létrehozása és adatbázisba feltöltése.
    /// </summary>
    /// <param name="projectView">Létrehozandó projekt</param>
    /// <returns>A létrehozott projekt</returns>
    Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel projectView);

    /// <summary>
    /// Projekt létrehozása és a felhasználó hozzárendelése a projekthez.
    /// </summary>
    /// <param name="projectView">Létrehozandó projekt</param>
    /// <param name="userName">Felhasználónév</param>
    /// <returns>A létrehozott projekt a hozzárendelt felhasználóval</returns>
    Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel projectView, string userName);

    /// <summary>
    /// Projekt hozzárendelése felhasználóhoz.
    /// </summary>
    /// <param name="projectid">Projekt egyedi azonosítója</param>
    /// <param name="userName">Felhasználónév</param>
    /// <returns>True, ha sikeres a hozzárendelés, false egyébként</returns>
    Task<bool> AssignProjectUserAsync(long projectid, string userName);
}
