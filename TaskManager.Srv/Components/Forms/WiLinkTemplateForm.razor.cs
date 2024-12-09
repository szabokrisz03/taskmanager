using FluentValidation;

using Microsoft.AspNetCore.Components;

using MudBlazor;

using System.Collections.Immutable;

using TaskManager.Srv.Model.Validation;
using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.AzdoServices;

namespace TaskManager.Srv.Components.Forms;

public partial class WiLinkTemplateForm
{
    private string[] _errors = new string[0];
    private ImmutableArray<string> Errors => _errors.ToImmutableArray();
    [Parameter] public WiLinkTemplateViewModel Model { get; set; } = new();
    [Parameter] public EventCallback<bool> OnValidate { get; set; }
    [Inject] private WiLinkTemplateValidation Validator { get; set; } = null!;
    [Inject] private IAzdoTeamProjectService TeamProjectService { get; set; } = null!;

    /// <summary>
    /// Validálás.
    /// </summary>
    public Func<object, string, Task<IEnumerable<string>>> FieldValidator => async (model, field) =>
    {
        var result = await Validator.ValidateAsync(ValidationContext<WiLinkTemplateViewModel>.CreateWithOptions((WiLinkTemplateViewModel)model, x => x.IncludeProperties(field)));
        _errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
        await OnValidate.InvokeAsync(result.IsValid);
        return result.IsValid ? Array.Empty<string>() : _errors;
    };

    /// <summary>
    /// Teamprojekt keresése.
    /// </summary>
    /// <param name="query">Lekérdezés</param>
    /// <returns>Stringeket tartalmazó tömb</returns>
    private async Task<IEnumerable<string>> SearchTeamProject(string? query)
    {
        var all = await TeamProjectService.GetTeamProjects();
        query ??= "";
        return all.Where(p => p.Name.ToLower().Contains(query.ToLower())).Select(p => p.Name);
    }
}
