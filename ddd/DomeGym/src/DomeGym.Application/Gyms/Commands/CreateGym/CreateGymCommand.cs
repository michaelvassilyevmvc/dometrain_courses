using DomeGym.Domain.GymAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Commands.CreateGym;

public record CreateGymCommand(string Name, Guid SubscriptionId)
    : IRequest<ErrorOr<Gym>>;