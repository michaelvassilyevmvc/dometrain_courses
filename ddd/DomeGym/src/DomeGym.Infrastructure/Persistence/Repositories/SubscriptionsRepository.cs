using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SubscriptionAggregate;
using Microsoft.EntityFrameworkCore;

namespace DomeGym.Infrastructure.Persistence.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly DomeGymDbContext _dbContext;

    public SubscriptionsRepository(DomeGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Subscriptions.AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await _dbContext.Subscriptions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == subscriptionId);
    }

    public async Task<List<Subscription>> ListAsync()
    {
        return await _dbContext.Subscriptions.AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _dbContext.Subscriptions.Update(subscription);
        await _dbContext.SaveChangesAsync();
    }
}