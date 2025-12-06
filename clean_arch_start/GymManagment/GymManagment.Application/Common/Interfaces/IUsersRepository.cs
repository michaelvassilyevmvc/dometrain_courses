using GymManagment.Domain.Users;

namespace GymManagment.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task AddUserAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
}