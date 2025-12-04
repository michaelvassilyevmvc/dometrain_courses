using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    void AdbSubscription(Subscription subscription);
}