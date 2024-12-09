using MudBlazor;

using TaskManager.Srv.Components.Dialogs;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Services.WiLinkService;

public class WiLinkTemplateViewService : IWiLinkTemplateViewService
{
    private readonly IDialogService dialogService;

    public WiLinkTemplateViewService(
        IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    /// <inheritdoc cref="IWiLinkTemplateViewService.CreateTemplateDialog"/>
    public async Task CreateTemplateDialog()
    {
        var dialog = await dialogService.ShowAsync<WiLinkTemplateEditDialog>("Új Sablon");
        await dialog.Result;
    }

    /// <inheritdoc cref="IWiLinkTemplateViewService.UpdateTemplateDialog(WiLinkTemplateViewModel)"/>
    public async Task UpdateTemplateDialog(WiLinkTemplateViewModel viewModel)
    {
        var parameters = new DialogParameters
        {
            ["ViewModel"] = viewModel
        };

        var dialog = await dialogService.ShowAsync<WiLinkTemplateEditDialog>("Sablon módosítása", parameters);
        await dialog.Result;
    }
}
