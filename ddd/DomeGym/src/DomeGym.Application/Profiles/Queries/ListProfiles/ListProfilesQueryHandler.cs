using DomeGym.Application.Common.Interfaces;
using DomeGym.Application.Profiles.Common;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Profiles.Queries.ListProfiles;

public class ListProfilesQueryHandler : IRequestHandler<ListProfilesQuery, ErrorOr<List<Profile?>>>
{
    private readonly ITrainersRepository _trainersRepository;
    private readonly IParticipantsRepository _participantsRepository;
    private readonly IAdminsRepository _adminsRepository;

    public ListProfilesQueryHandler(
        ITrainersRepository trainersRepository,
        IParticipantsRepository participantsRepository,
        IAdminsRepository adminsRepository)
    {
        _trainersRepository = trainersRepository;
        _participantsRepository = participantsRepository;
        _adminsRepository = adminsRepository;
    }

    public async Task<ErrorOr<List<Profile?>>> Handle(ListProfilesQuery query, CancellationToken cancellationToken)
    {
        var trainerProfile = await _trainersRepository.GetProfileByUserIdAsync(query.UserId);
        var adminProfile = await _adminsRepository.GetProfileByUserIdAsync(query.UserId);
        var participantProfile = await _participantsRepository.GetProfileByUserIdAsync(query.UserId);

        var profiles = new[]
            {
                trainerProfile, adminProfile, participantProfile
            }
            .Where(profile => profile is not null)
            .ToList();

        if (profiles.Count == 0)
        {
            return Error.NotFound("User not found");
        }

        return profiles;
    }
}