using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Gyms.Queries.ListGyms;

public record ListGymsQuery(Guid SubscriptionId) : IRequest<ErrorOr<List<Gym>>>;