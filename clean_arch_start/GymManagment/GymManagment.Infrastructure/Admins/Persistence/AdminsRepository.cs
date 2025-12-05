using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins;
using GymManagment.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Admins.Persistence;

public class AdminsRepository : IAdminsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public AdminsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Admin?> GetByIdAsync(Guid adminId)
    {
        return await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == adminId);
    }

    public Task UpdateAsync(Admin admin)
    {
        _dbContext.Admins.Update(admin);
        return Task.CompletedTask;
    }
}