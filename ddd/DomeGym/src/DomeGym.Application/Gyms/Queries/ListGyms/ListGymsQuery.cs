using DomeGym.Domain.GymAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Queries.ListGyms;

public record ListGymsQuery(Guid SubscriptionId): IRequest<ErrorOr<List<Gym>>>;