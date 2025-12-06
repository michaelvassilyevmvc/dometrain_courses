using GymManagment.Application.Subscriptions.Commands.CreateSubscription;
using GymManagment.Domain.Subscriptions;
using TestCommon.TestContants;

namespace TestCommon.Subscriptions;

public static class SubscriptionCommandFactory
{
    public static CreateSubscriptionCommand CreateSubscriptionCommand(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null
    )
    {
        return new CreateSubscriptionCommand(
            subscriptionType ?? TestCommon.TestContants.Constants.Subscriptions.DefaultSubscriptionType,
            adminId ?? Constants.Admin.Id
        );
    }
}