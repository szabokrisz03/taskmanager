using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.WiLinkService;

/// <summary>
/// "WiTemplate" létrehozására, kezelésére. 
/// </summary>
public interface IWiLinkTemplateService
{
    /// <summary>
    /// "WiTemplate" listázása
    /// </summary>
    /// <param name="projectId">Projekt egyedi azonosítója</param>
    /// <param name="take">Eltett</param>
    /// <param name="skip">Kihagyott</param>
    /// <returns>"WiTemplate"-t tartalmazó lista</returns>
    Task<List<WiLinkTemplateViewModel>> ListTemplates(long projectId, int take = 10, int skip = 0);

    /// <summary>
    /// Megszámolja, hogy az adott projektben hány darab "WiTemplate" van.
    /// </summary>
    /// <param name="projectId">Projekt egyedi azonosítója</param>
    /// <returns>Darabszám</returns>
    Task<int> CountTemplates(long projectId);

    /// <summary>
    /// "WiTemplate" létrehozása.
    /// </summary>
    /// <param name="template">A létrehozandó "WiTemplate"</param>
    Task CreateTemplate(WiLinkTemplateViewModel template);

    /// <summary>
    /// "WiTemplate" frissítése.
    /// </summary>
    /// <param name="template">A frissítendő "WiTemplate"</param>
    Task UpdateTemplate(WiLinkTemplateViewModel template);

    /// <summary>
    /// "WiTemplate" törlése.
    /// </summary>
    /// <param name="templateId">"WiTemplate" egyedi azonosítója</param>
    Task DeleteTemplate(long templateId);
}
