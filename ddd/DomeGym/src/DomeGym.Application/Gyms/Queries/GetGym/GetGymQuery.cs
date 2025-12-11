using DomeGym.Domain.GymAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Queries.GetGym;

public record GetGymQuery(Guid SubscriptionId, Guid GymId): IRequest<ErrorOr<Gym>>;