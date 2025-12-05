using GymManagment.Domain.Subscriptions;
using TestCommon.TestContants;

namespace TestCommon.Subscriptions;

public static class SubscriptionFactory
{
    public static Subscription CreateSubscription(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null,
        Guid? id = null)
    {
        return new Subscription(
            subscriptionType: subscriptionType ?? Contants.Subscriptions.DefaultSubscriptionType,
            adminId: adminId ?? Contants.Admin.Id,
            id: id ?? Contants.Subscriptions.Id
        );
    }
}