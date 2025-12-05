using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AdbSubscriptionAsync(Subscription subscription);
    Task<bool> ExistAsync(Guid id);
    Task<Subscription?> GetByAdminIdAsync(Guid adminId);
    Task<Subscription?> GetByIdAsync(Guid id);
    Task<List<Subscription>> ListAsync();
    Task RemoveSubscriptionAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}