using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Subscriptions;
using GymManagment.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public SubscriptionsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdbSubscriptionAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(subscription => subscription.Id == id);
    }

    public async Task<Subscription?> GetByAdminIdAsync(Guid adminId)
    {
        return await _dbContext.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AdminId == adminId);
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await _dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId);
    }

    public async Task<List<Subscription>> ListAsync()
    {
        return await _dbContext.Subscriptions.ToListAsync();
    }

    public Task RemoveSubscriptionAsync(Subscription subscription)
    {
        _dbContext.Remove(subscription);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Subscription subscription)
    {
        _dbContext.Update(subscription);
        return Task.CompletedTask;
    }
}