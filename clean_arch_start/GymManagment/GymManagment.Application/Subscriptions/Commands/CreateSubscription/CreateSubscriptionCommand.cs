using MediatR;
using ErrorOr;
using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubcriptionType, Guid AdminId) : IRequest<ErrorOr<Subscription>>;