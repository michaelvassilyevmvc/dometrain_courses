using DomeGym.Domain.SubscriptionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubscriptionType, Guid AdminId): IRequest<ErrorOr<Subscription>>;