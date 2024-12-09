using FluentValidation;

using TaskManager.Srv.Model.ViewModel;
using TaskManager.Srv.Services.TaskServices;

namespace TaskManager.Srv.Model.Validation;

/// <summary>
/// Feladat validálása
/// </summary>
public class TaskValidator : AbstractValidator<TaskViewModel>
{
    public TaskValidator(ITaskDisplayService taskDisplayService)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("A név kitöltése kötelező")
            .NotNull().WithMessage("A név kitöltése kötelező");

        RuleFor(x => x.Name)
            .CustomAsync(async (name, context, _) =>
            {
                var dto = context.InstanceToValidate;
                if (await taskDisplayService.TaskNameExistsAsync(dto.ProjectId, dto.Name))
                {
                    context.AddFailure("Ezzel a névvel már létezik feladat a projektben!");
                }
            });
    }
}