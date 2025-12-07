using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Authorization;

namespace GymManagment.Application.Gyms.Commands.CreateGym;

[Authorize(Roles = "Admin")]
public record CreateGymCommand(string Name, Guid SubscriptionId): IRequest<ErrorOr<Gym>>;