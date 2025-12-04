using GymManagment.Domain.Subscriptions;

namespace GymManagment.Contracts.Subscriptions;

public record SubscriptionResponse(
    Guid Id,
    SubcriptionType SubcriptionType);   