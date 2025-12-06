using GymManagment.Application.Authentication.Common;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password): IRequest<ErrorOr<AuthenticationResult>>;
