using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;

namespace GymManagment.Application.Profiles.Queries.ListProfiles;

public class ListProfilesQueryHandler(
    IUsersRepository _usersRepository
    ): IRequestHandler<ListProfilesQuery, ErrorOr<ListProfilesResult>>
{
    public async Task<ErrorOr<ListProfilesResult>> Handle(ListProfilesQuery query, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(query.UserId);
        if (user is null)
        {
            return Error.NotFound("User not found");
        }

        return new ListProfilesResult(
            user.AdminId,
            user.ParticipantId,
            user.TrainerId);
    }
}