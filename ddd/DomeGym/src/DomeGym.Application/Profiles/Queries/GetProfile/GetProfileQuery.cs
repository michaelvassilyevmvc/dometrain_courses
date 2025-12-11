using DomeGym.Application.Profiles.Common;
using MediatR;
using ErrorOr;
using DomeGym.Domain.Profiles;

namespace DomeGym.Application.Profiles.Queries.GetProfile;

public record GetProfileQuery(Guid UserId, ProfileType ProfileType): IRequest<ErrorOr<Profile?>>;