namespace TaskManager.Srv.Services.WiServices;

/// <summary>
/// Workitem létrehozására, lekérdezésére.
/// </summary>
public interface IWiService
{
    /// <summary>
    /// Workitem lértehozása.
    /// </summary>
    /// <param name="wiId">Workitem egyedi azonosítója</param>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    Task CreateWiAsync(int wiId, long taskId);

    /// <summary>
    /// Workitemek lekérdezése.
    /// </summary>
    /// <param name="taskId">Feladat egyedi azonosítója</param>
    /// <returns>Workitem id-ket tartalmazó tömb</returns>
    Task<int[]> ListWorkItem(long taskId);

    /// <summary>
    /// Workitem törlését végzi el adatbázisban
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteWi(int id);
}
