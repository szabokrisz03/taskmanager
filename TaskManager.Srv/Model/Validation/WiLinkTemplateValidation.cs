using FluentValidation;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.AzdoServices;

namespace TaskManager.Srv.Model.Validation;

/// <summary>
/// "WiTemplate" validásása.
/// </summary>
public class WiLinkTemplateValidation : AbstractValidator<WiLinkTemplateViewModel>
{
    private readonly IAzdoTeamProjectService teamsService;

    public WiLinkTemplateValidation(
        IAzdoTeamProjectService teamsService)
    {
        this.teamsService = teamsService;

        MakeRules();
    }

    private void MakeRules()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Kötelező kitölteni")
            .NotNull().WithMessage("Kötelező kitölteni");

        RuleFor(t => t.TeamProject)
            .NotEmpty().WithMessage("Kötelező kitölteni")
            .NotNull().WithMessage("Kötelező kitölteni")
            .MustAsync(async (t, _) =>
            {
                var teamProjects = await teamsService.GetTeamProjects();
                return teamProjects.Any(p => p.Name == t);
            }).WithMessage("Csak létező TP használható!");
    }
}