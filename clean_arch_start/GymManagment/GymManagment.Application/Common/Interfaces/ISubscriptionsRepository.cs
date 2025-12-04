using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AdbSubscriptionAsync(Subscription subscription);
    Task<Subscription?> GetByIdAsync(Guid id);
}