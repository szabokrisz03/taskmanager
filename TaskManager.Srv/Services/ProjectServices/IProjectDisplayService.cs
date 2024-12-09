using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.ProjectServices;

/// <summary>
/// Projekttel kapcsolatos ellenőrzésekre.
/// </summary>
public interface IProjectDisplayService
{
    /// <summary>
    /// Megnézi, hogy a megadott projektId létre van-e már hozva az adatbázisban.
    /// </summary>
    /// <param name="rowid">Projekt egyedi azonosítója</param>
    /// <returns>True, ha van már, false egyébként</returns>
    Task<bool> ProjectIdExistsAsync(long rowid);

    /// <summary>
    /// Az összes hozzáadott projekt kilistázása adatbázisból.
    /// </summary>
    /// <param name="searchTerm">A keresett projekt</param>
    /// <param name="take">Eltett</param>
    /// <param name="skip">Kihagyott</param>
    /// <returns>Projekteket tartalmazó lista</returns>
    Task<List<ProjectViewModel>> ListProjectsAsync(string searchTerm, int take, int skip = 0);

    /// <summary>
    /// A felhasználó saját projektjeinek kilistázása.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    /// <param name="searchTerm">A keresett projekt</param>
    /// <param name="take">Eltett</param>
    /// <param name="skip">Kihagyott</param>
    /// <returns>Projekteket tartalmazó lista</returns>
    Task<List<ProjectViewModel>> ListUserProjectsAsync(string userName, string searchTerm, int take, int skip = 0);

    /// <summary>
    /// A felhasználó saját projektjeinek kilistázása.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    /// <param name="visitedUntil">Dátum</param>
    /// <param name="take">Eltett</param>
    /// <param name="skip">Kihagyott</param>
    /// <returns>Projekteket tartalmazó lista</returns>
    Task<List<ProjectViewModel>> ListUserProjectsAsync(string userName, DateTime visitedUntil, int take, int skip = 0);

    /// <summary>
    /// Projekt lekérdezése az adatbázisból.
    /// </summary>
    /// <param name="technicalName">Egyedi azonosító</param>
    /// <returns>A lekérdezett projekt</returns>
    Task<ProjectViewModel?> GetProjectAsync(Guid technicalName);

    /// <summary>
    /// projektId-t lekérdezi az adatbázisból.
    /// </summary>
    /// <param name="technicalName">Egyedi azonosító</param>
    /// <returns>A projekt ID-ja</returns>
    Task<long> GetProjectIdAsync(Guid technicalName);

    /// <summary>
    /// A projekt nevét kérdezi le adatbázisból.
    /// </summary>
    /// <param name="technicalName">Egyedi azonosító</param>
    /// <returns>A projekt neve</returns>
    Task<string?> GetProjectNameAsync(Guid technicalName);

    /// <summary>
    /// Projekt lekérdezése az adatbázisból.
    /// </summary>
    /// <param name="rowId">Projekt egyedi azonosítója</param>
    /// <returns>A lekérdezett projekt</returns>
    Task<ProjectViewModel?> GetProjectAsync(long rowId);

    /// <summary>
    /// Megnézi, hogy a megadott "technicalName" létre van-e már hozva az adatbázisban.
    /// </summary>
    /// <param name="technicalName">Egyedi azonosító</param>
    /// <returns>True, ha van már, false egyébként</returns>
    Task<bool> ProjectTechnicalNameExistsAsync(Guid technicalName);

    /// <summary>
    /// Megnézi, hogy a megadott névvel szerepel-e már projekt az adatbázisban.
    /// </summary>
    /// <param name="name">Projekt neve</param>
    /// <returns>True, ha szerepel, false egyébként</returns>
    Task<bool> ProjectNameExistsAsync(string name);
}
