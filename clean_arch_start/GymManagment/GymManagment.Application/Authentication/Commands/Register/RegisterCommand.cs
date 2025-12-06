using ErrorOr;
using GymManagment.Application.Authentication.Common;
using MediatR;

namespace GymManagment.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password): IRequest<ErrorOr<AuthenticationResult>>;