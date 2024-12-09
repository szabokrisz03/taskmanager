namespace TaskManager.Srv.Services.ProjectServices;

/// <summary>
/// MEssage-ek és formok megjelenítésére
/// </summary>
public interface IProjectViewService
{
    /// <summary>
    /// Dialógus elkészítése projekt létrehozásánál.
    /// </summary>
    /// <param name="userName">Felhasználónév</param>
    Task CreateProjectAsync(string userName = "");
}
