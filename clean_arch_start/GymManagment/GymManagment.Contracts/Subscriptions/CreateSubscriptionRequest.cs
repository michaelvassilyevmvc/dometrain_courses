namespace GymManagment.Contracts.Subscriptions;

public record CreateSubscriptionRequest(
    SubscriptionType SubscriptionType,
    Guid AdminId);