using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Users;
using GymManagment.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Users;

public class UsersRepository(GymManagementDbContext _dbContext) : IUsersRepository
{
    public async Task AddUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public Task UpdateAsync(User user)
    {
        _dbContext.Update(user);
        return Task.CompletedTask;
    }
}