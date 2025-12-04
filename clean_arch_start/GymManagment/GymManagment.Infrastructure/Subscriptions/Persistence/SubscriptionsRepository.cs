using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Subscriptions;

namespace GymManagment.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly static List<Subscription> _subscriptions = new();

    public Task AdbSubscriptionAsync(Subscription subscription)
    {
        _subscriptions.Add(subscription);
        return Task.CompletedTask;
    }

    public Task<Subscription?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_subscriptions.FirstOrDefault(s => s.Id == id));
    }
}