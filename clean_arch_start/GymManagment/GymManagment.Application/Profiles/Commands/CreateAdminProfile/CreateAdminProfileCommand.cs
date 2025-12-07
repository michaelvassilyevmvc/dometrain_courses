using ErrorOr;
using MediatR;

namespace GymManagment.Application.Profiles.Commands.CreateAdminProfile;

public record CreateAdminProfileCommand(Guid UserId) : IRequest<ErrorOr<Guid>>;