using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Participants.Commands.CancelReservation;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ErrorOr<Deleted>>
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IParticipantsRepository _participantsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelReservationCommandHandler(ISessionsRepository sessionsRepository,
        IParticipantsRepository participantsRepository, IDateTimeProvider dateTimeProvider)
    {
        _sessionsRepository = sessionsRepository;
        _participantsRepository = participantsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Deleted>> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var session = await _sessionsRepository.GetByIdAsync(command.SessionId);
        if (session is null)
        {
            return Error.NotFound("User doesn't have a reservation for the given session");
        }

        if (!session.HasReservationForParticipant(command.ParticipantId))
        {
            return Error.NotFound("User doesn't have a reservation for the given session");
        }

        var participant = await _participantsRepository.GetByIdAsync(command.ParticipantId);

        if (participant is null)
        {
            return Error.NotFound("Participant not found");
        }

        if (!participant.HasReservationForSession(session.Id))
        {
            return Error.Unexpected("Participant expected to have reservation to session.");
        }

        var cancelReservationResult = session.CancelReservation(participant, dateTimeProvider: _dateTimeProvider);
        if (cancelReservationResult.IsError)
        {
            return cancelReservationResult.Errors;
        }

        await _sessionsRepository.UpdateAsync(session);
        return Result.Deleted;
    }
}