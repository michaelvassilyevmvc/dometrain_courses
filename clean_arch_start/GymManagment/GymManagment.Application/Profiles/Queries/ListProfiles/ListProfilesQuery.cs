using MediatR;
using ErrorOr;

namespace GymManagment.Application.Profiles.Queries.ListProfiles;

public record ListProfilesQuery(Guid UserId)
    : IRequest<ErrorOr<ListProfilesResult>>;