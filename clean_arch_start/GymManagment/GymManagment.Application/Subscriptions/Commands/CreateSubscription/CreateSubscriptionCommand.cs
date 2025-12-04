using MediatR;
using ErrorOr;
using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubcriptionType, Guid AdminId) : IRequest<ErrorOr<Subscription>>;