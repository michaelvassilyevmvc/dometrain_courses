using GymManagment.Application.Common.Interfaces;
using GymManagment.Application.Common.Models;
using Throw;

namespace GymManagment.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor)
    : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();
        var claim = _httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == "id");
        return new CurrentUser(Guid.Parse(claim.Value));
    }
}