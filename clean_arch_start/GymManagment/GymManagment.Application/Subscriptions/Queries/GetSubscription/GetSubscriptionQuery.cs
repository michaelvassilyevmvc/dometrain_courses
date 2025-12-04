using GymManagment.Domain.Subscriptions;
using ErrorOr;
using MediatR;

namespace GymManagment.Application.Subscriptions.Queries.GetSubscription;

public record GetSubscriptionQuery(Guid SubscriptionId) : IRequest<ErrorOr<Subscription>>;