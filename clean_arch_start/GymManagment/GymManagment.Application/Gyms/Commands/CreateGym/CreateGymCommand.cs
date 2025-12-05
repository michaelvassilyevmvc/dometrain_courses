using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Gyms.Commands.CreateGym;

public record CreateGymCommand(string Name, Guid SubscriptionId): IRequest<ErrorOr<Gym>>;