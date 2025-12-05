using ErrorOr;
using MediatR;

namespace GymManagment.Application.Subscriptions.Commands.DeleteSubscription;

public record DeleteSubscriptionCommand(Guid SubscriptionId): IRequest<ErrorOr<Deleted>>;