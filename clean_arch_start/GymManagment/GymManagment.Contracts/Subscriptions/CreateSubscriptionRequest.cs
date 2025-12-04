using GymManagment.Domain.Subscriptions;

namespace GymManagment.Contracts.Subscriptions;

public record CreateSubscriptionRequest(
    SubcriptionType SubcriptionType,
    Guid AdminId);