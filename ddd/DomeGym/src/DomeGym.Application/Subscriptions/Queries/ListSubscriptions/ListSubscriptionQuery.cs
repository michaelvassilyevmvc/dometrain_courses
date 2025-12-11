using DomeGym.Domain.SubscriptionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Subscriptions.Queries.ListSubscriptions;

public record ListSubscriptionQuery(): IRequest<ErrorOr<List<Subscription>>>;