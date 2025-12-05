using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Gyms.Queries.GetGym;

public record GetGymQuery(Guid SubscriptionId, Guid GymId): IRequest<ErrorOr<Gym>>;