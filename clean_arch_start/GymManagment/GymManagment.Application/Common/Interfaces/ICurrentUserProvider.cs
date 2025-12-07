using GymManagment.Application.Common.Models;

namespace GymManagment.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}