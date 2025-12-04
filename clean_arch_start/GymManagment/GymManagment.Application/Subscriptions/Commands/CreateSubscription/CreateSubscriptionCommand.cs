using MediatR;
using ErrorOr;

namespace GymManagment.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubcriptionType, Guid AdminId) : IRequest<ErrorOr<Guid>>;