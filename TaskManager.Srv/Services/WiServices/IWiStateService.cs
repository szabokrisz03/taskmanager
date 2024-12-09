using TaskManager.Srv.Model.DTO;

namespace TaskManager.Srv.Services.WiServices;

/// <summary>
/// Workitemek részletezésére.
/// </summary>
public interface IWiStateService
{
    /// <summary>
    /// Workitemek részleteinek hozzárendelése a workitemhez.
    /// </summary>
    /// <param name="source">Workitem id-ket tartalmazó tömb</param>
    /// <param name="collector">Workitemeket tartalmazó lista</param>
    void PropertyWis(IEnumerable<int> source, List<WorkItem> collector);

    /// <summary>
    /// Workitemek részleteinek hozzárendelése a workitemhez.
    /// </summary>
    /// <param name="sources">Workitem id-ket tartalmazó tömb</param>
    /// <param name="capacity">Kapacitás</param>
    /// <returns></returns>
    List<WorkItem> DetailWIs(IEnumerable<int> sources, int capacity = 0);

    /// <summary>
    /// Kapcsolódó workitemek kilistázása.
    /// </summary>
    /// <param name="wiId">Workitem egyedi azonosítója</param>
    /// <returns>Workitemeket tartalmazó lista.</returns>
    List<WorkItem> ListConnectingWis(int wiId);

    /// <summary>
    /// Linkelt workitemek lekérdezése.
    /// </summary>
    /// <param name="source">Workitem id-ket tartalmazó tömb</param>
    /// <returns>Kulcs-érték párokat tartalmazó dictionary, ahol a kulcs a parent workitem, az érték a linkelt workitemeket tartalmazó lista</returns>
    Dictionary<WorkItem, List<WorkItem>> queryMaker(IEnumerable<int> source);
}
