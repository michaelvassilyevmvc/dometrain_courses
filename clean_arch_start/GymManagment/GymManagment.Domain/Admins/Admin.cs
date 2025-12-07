using GymManagment.Domain.Admins.Events;
using GymManagment.Domain.Common;
using GymManagment.Domain.Subscriptions;
using Throw;

namespace GymManagment.Domain.Admins;

public class Admin: Entity
{
    private Admin()
    {
    }

    public Admin(
        Guid userId,
        Guid? subscriptionId = null,
        Guid? id = null
        ): base(id ?? Guid.NewGuid())
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
    }

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
        _domainEvents.Add(new SubscriptionDeletedEvent(subscriptionId));
    }
}