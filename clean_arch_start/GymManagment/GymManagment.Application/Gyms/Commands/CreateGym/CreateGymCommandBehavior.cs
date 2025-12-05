using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandBehavior : IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(
        CreateGymCommand request,
        RequestHandlerDelegate<ErrorOr<Gym>> next,
        CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.Select(error =>
                    Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToList();
        }

        return await next();
    }
}