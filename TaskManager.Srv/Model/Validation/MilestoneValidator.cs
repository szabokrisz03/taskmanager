using FluentValidation;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.MilestoneServices;

namespace TaskManager.Srv.Model.Validation;

/// <summary>
/// Határidők validálása.
/// </summary>
public class MilestoneValidator : AbstractValidator<MilestoneViewModel>
{
    public MilestoneValidator(
    IMilestoneDisplayService milestoneDisplayService)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("A név kitöltése kötelező")
            .NotNull().WithMessage("A név kitöltése kötelező");

        RuleFor(x => x.Name)
            .CustomAsync(async (name, context, _) =>
            {
                var dto = context.InstanceToValidate;
                if (await milestoneDisplayService.MilestoneNameExistsAsync(dto.TaskId, dto.Name))
                {
                    context.AddFailure("Ezzel a névvel már létezik mérföldkő a feladatban!");
                }
            });

        RuleFor(d => d.Planned)
            .NotEmpty().WithMessage("A dátum kitölrése kötelező")
            .NotNull().WithMessage("A dátum kitöltése kötelező");

        RuleFor(d => d.Planned)
            .GreaterThanOrEqualTo(p => DateTime.Today).WithMessage("Nem állíthatsz be régebbi dátumot!");
    }
}