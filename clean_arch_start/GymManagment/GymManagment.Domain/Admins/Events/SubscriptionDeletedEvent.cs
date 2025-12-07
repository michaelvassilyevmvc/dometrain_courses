using GymManagment.Domain.Common;

namespace GymManagment.Domain.Admins.Events;

public record SubscriptionDeletedEvent(Guid SubscriptionId): IDomainEvent;
