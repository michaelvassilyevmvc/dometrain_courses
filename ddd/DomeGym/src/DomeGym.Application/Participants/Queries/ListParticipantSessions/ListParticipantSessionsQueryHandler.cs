using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Participants.Queries.ListParticipantSessions;

public class ListParticipantSessionsQueryHandler : IRequestHandler<ListParticipantSessionsQuery, ErrorOr<List<Session>>>
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IParticipantsRepository _participantsRepository;

    public ListParticipantSessionsQueryHandler(ISessionsRepository sessionsRepository,
        IParticipantsRepository participantsRepository)
    {
        _sessionsRepository = sessionsRepository;
        _participantsRepository = participantsRepository;
    }

    public async Task<ErrorOr<List<Session>>> Handle(ListParticipantSessionsQuery query,
        CancellationToken cancellationToken)
    {
        var participant = await _participantsRepository.GetByIdAsync(query.ParticipantId);
        if (participant is null)
        {
            return Error.NotFound("Participant not found");
        }

        return await _sessionsRepository.ListByIdsAsync(participant.SessionIds, startDateTime: query.StartDateTime,
            endDateTime: query.EndDateTime);
    }
}