using GymManagment.Domain.Users;

namespace GymManagment.Application.Authentication.Common;

public record AuthenticationResult(User User, string Token);