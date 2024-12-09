using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Components.Forms;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.WiLinkService;

namespace TaskManager.Srv.Components.Dialogs;

/// <summary>
/// "WiTemplate" dialógusa.
/// </summary>
public partial class WiLinkTemplateEditDialog
{
    private bool _disableSubmit = false;
    [Parameter] public WiLinkTemplateViewModel ViewModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance Dialog { get; set; } = null!;
    [Inject] public IWiLinkTemplateService TemplateService { get; set; } = null!;

    private void OnValidate(bool isValid)
    {
        _disableSubmit = !isValid;
        StateHasChanged();
    }

    /// <summary>
    /// Megszakítás.
    /// </summary>
    private void Cancel()
    {
        Dialog.Cancel();
    }

    /// <summary>
    /// "WiTemplate" szerkesztése.
    /// </summary>
    private async Task EditTemplate()
    {
        if (ViewModel.RowId == 0)
        {
            await CreateTemplate();
        }
        else
        {
            await UpdateTemplate();
        }
    }

    /// <summary>
    /// "WiTemplate" dialógus létrehozása.
    /// </summary>
    private async Task CreateTemplate()
    {
        await TemplateService.CreateTemplate(ViewModel);
    }

    /// <summary>
    /// "WiTemplate" frissítése.
    /// </summary>
    private async Task UpdateTemplate()
    {
        await TemplateService.UpdateTemplate(ViewModel);
    }
}
