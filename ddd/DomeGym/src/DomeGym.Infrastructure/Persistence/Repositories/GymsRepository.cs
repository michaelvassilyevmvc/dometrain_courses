using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.GymAggregate;
using Microsoft.EntityFrameworkCore;

namespace DomeGym.Infrastructure.Persistence.Repositories;

public class GymsRepository: IGymsRepository
{
    private readonly DomeGymDbContext _dbContext;
    
    public GymsRepository(DomeGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddGymAsync(Gym gym)
    {
        await _dbContext.Gyms.AddAsync(gym);
    }

    public async Task<Gym?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Gyms.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Gyms.AnyAsync(x => x.Id == id);
    }

    public async Task<List<Gym>> ListSubscriptionGymsAsync(Guid subscriptionId)
    {
        return await _dbContext.Gyms.Where(x => x.SubscriptionId == subscriptionId).ToListAsync();
    }

    public async Task UpdateAsync(Gym gym)
    {
        _dbContext.Gyms.Update(gym);
        await _dbContext.SaveChangesAsync();
    }
}