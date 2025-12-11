using DomeGym.Application.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ErrorOr<Success>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IParticipantsRepository _participantsRepository;

    public CreateReservationCommandHandler(ISessionRepository sessionRepository,
        IParticipantsRepository participantsRepository)
    {
        _sessionRepository = sessionRepository;
        _participantsRepository = participantsRepository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(command.SessionId);
        if (session is null)
        {
            return Error.NotFound("Session not found");
        }

        if (session.HasReservationForParticipant(command.ParticipantId))
        {
            return Error.Conflict("Participant already has reservation");
        }

        var participant = await _participantsRepository.GetByIdAsync(command.ParticipantId);
        if (participant is null)
        {
            return Error.NotFound("Participant not found");
        }

        if (participant.HasReservationForSession(session.Id))
        {
            return Error.Unexpected("Participant not expected to have reservation to session.");
        }

        if (!participant.IsTimeSlotFree(session.Date, session.Time))
        {
            return Error.Conflict("Participant's calendar is not free for the entire session duration");
        }

        var reserveSpotResult = session.ReserveSpot(participant);
        if (reserveSpotResult.IsError)
        {
            return reserveSpotResult.Errors;
        }

        await _sessionRepository.UpdateAsync(session);
        return Result.Success;
    }
}