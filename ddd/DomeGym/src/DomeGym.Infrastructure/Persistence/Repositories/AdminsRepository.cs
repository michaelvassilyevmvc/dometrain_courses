using DomeGym.Application.Common.Interfaces;
using DomeGym.Application.Profiles.Common;
using DomeGym.Domain.AdminAggregate;
using DomeGym.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace DomeGym.Infrastructure.Persistence.Repositories;

public class AdminsRepository : IAdminsRepository
{
    private readonly DomeGymDbContext _dbContext;

    public AdminsRepository(DomeGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAdminAsync(Admin participatn)
    {
        await _dbContext.Admins.AddAsync(participatn);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Profile?> GetProfileByUserIdAsync(Guid userId)
    {
        var admin = await _dbContext.Admins.AsNoTracking()
            .FirstOrDefaultAsync(predicate: admin => admin.UserId == userId);
        return admin is null ? null : new Profile(admin.Id, ProfileType.Admin);
    }

    public async Task<Admin?> GetByIdAsync(Guid adminId)
    {
        return await _dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate: admin => admin.Id == adminId);
    }

    public async  Task UpdateAsync(Admin admin)
    {
         _dbContext.Admins.Update(admin);
         await _dbContext.SaveChangesAsync();
    }
}