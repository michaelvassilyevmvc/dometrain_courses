using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Gyms;
using GymManagment.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Gyms.Persistence;

public class GymsRepository : IGymsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public GymsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Gym?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Gyms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await _dbContext.Gyms.Where(x => x.SubscriptionId == subscriptionId)
            .ToListAsync();
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await _dbContext.Gyms.AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }

    public async Task AddGymAsync(Gym gym)
    {
        await _dbContext.Gyms.AddAsync(gym);
    }

    public Task UpdateAsync(Gym gym)
    {
        _dbContext.Update(gym);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Gym gym)
    {
        _dbContext.Gyms.Remove(gym);
        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(List<Gym> gyms)
    {
        _dbContext.Gyms.RemoveRange(gyms);
        return Task.CompletedTask;
    }
}