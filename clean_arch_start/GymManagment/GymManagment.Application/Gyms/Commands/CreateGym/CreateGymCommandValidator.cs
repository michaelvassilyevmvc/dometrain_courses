using FluentValidation;

namespace GymManagment.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandValidator: AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(100);
    }
}