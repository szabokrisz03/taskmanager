using FluentValidation;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.ProjectServices;

namespace TaskManager.Srv.Model.Validation;

/// <summary>
/// Projekt validálása.
/// </summary>
public class ProjectValidator : AbstractValidator<ProjectViewModel>
{
    public ProjectValidator(
        IProjectDisplayService projectDisplayService)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("A név kitöltése kötelező")

            .MustAsync(async (name, _) => !await projectDisplayService.ProjectNameExistsAsync(name))
            .WithMessage("Ilyen néven már létezik projekt!");
    }
}