using DomeGym.Application.Profiles.Common;
using MediatR;
using ErrorOr;
namespace DomeGym.Application.Profiles.Queries.ListProfiles;

public record ListProfilesQuery(Guid UserId): IRequest<ErrorOr<List<Profile>>>;