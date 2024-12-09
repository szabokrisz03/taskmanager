using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.WiLinkService;

/// <summary>
/// "WiTemplate" dialógus létrehozására.
/// </summary>
public interface IWiLinkTemplateViewService
{
    /// <summary>
    /// "WiTemplate" dialógus létrehozása.
    /// </summary>
    Task CreateTemplateDialog();

    /// <summary>
    /// "WiTemplate" dialógus frissítése.
    /// </summary>
    /// <param name="viewModel">A frissítendő "WiTemplate"</param>
    Task UpdateTemplateDialog(WiLinkTemplateViewModel viewModel);
}
