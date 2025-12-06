using GymManagment.Application.Authentication.Common;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository
) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(query.Email);

        return user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(user, _jwtTokenGenerator.GenerateToken(user));
    }
}