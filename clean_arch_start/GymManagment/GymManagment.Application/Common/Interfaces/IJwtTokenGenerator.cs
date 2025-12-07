using GymManagment.Domain.Users;

namespace GymManagment.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}