using GymManagment.Domain.Subscriptions;
using Throw;

namespace GymManagment.Domain.Admins;

public class Admin
{
    private Admin()
    {
    }

    public Admin(
        Guid userId,
        Guid? subscriptionId = null,
        Guid? id = null
        )
    {
        Id = id ?? Guid.NewGuid();
        SubscriptionId = subscriptionId;
        UserId = userId;
    }

    public Guid Id { get; private set; }
    public Guid? SubscriptionId { get; private set; } = null;
    public Guid UserId { get; }

    public void SetSubscription(Subscription subscription)
    {
        SubscriptionId.HasValue.Throw()
            .IfTrue();
        SubscriptionId = subscription.Id;
    }
    
    public void DeleteSubscription(Guid subscriptionId)
    {
        SubscriptionId.ThrowIfNull()
            .IfNotEquals(subscriptionId);
        SubscriptionId = null;
    }
}