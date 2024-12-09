using Microsoft.AspNetCore.Components;

using MudBlazor;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;
using TaskManager.Srv.Services.WiLinkService;

namespace TaskManager.Srv.Pages.Projects;

/// <summary>
/// "WiTemplate" megjelenítése.
/// </summary>
public partial class ProjectWiTemplates
{
    private ProjectViewModel project = new();
    [Parameter] public string TechnicalName { get; set; } = "";
    [Inject] private IProjectDisplayService ProjectDisplayService { get; set; } = null!;
    [Inject] private IWiLinkTemplateService DataService { get; set; } = null!;
    [Inject] private IWiLinkTemplateViewService TemplateViewService { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        project.RowId = 0;
        if (TechnicalName != null && Guid.TryParse(TechnicalName, out Guid technicalNameGuid))
        {
            project = await ProjectDisplayService.GetProjectAsync(technicalNameGuid) ?? new();
        }
    }

    /// <summary>
    /// Táblázat feltöltése.
    /// </summary>
    /// <param name="state">State</param>
    /// <returns>TableData<WiLinkTemplateViewModel></returns>
    private async Task<TableData<WiLinkTemplateViewModel>> LoadData(TableState state)
    {
        int skip = state.PageSize * state.Page;
        int take = state.PageSize;

        int size = await DataService.CountTemplates(project.RowId);
        var templates = await DataService.ListTemplates(project.RowId, take, skip);

        return new TableData<WiLinkTemplateViewModel>
        {
            Items = templates,
            TotalItems = size
        };
    }

    /// <summary>
    /// "WiTemplate" létrehozása.
    /// </summary>
    private async Task CreateTemplate()
    {
        await TemplateViewService.CreateTemplateDialog();
    }
}