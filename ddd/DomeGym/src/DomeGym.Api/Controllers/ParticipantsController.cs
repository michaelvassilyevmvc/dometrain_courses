using DomeGym.Application.Participants.Commands.CancelReservation;
using DomeGym.Application.Participants.Queries.ListParticipantSessions;
using DomeGym.Application.Reservations.Commands.CreateReservation;
using DomeGym.Contracts.Sessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace DomeGym.Api.Controllers;

[Route("participants")]
public class ParticipantsController : ApiController
{
    private readonly ISender serder;

    public ParticipantsController(ISender serder)
    {
        this.serder = serder;
    }

    [HttpGet("{participantId}/sessions")]
    public async Task<IActionResult> ListParticipantSessions(
        Guid participantId,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null)
    {
        var command = new ListParticipantSessionsQuery(
            participantId,
            startDateTime,
            endDateTime);

        var listParticipantSessionsResult = await serder.Send(command);
        return listParticipantSessionsResult.Match(
            sessions => Ok(sessions.ConvertAll(session => new SessionResponse(
                session.Id,
                session.Name,
                session.Description,
                session.NumParticipants,
                session.MaxParticipants,
                session.Date.ToDateTime(session.Time.Start),
                session.Date.ToDateTime(session.Time.End),
                session.Categories.Select(category => category.Name)
                    .ToList()
            ))),
            Problem
        );
    }

    [HttpDelete("{participantId:guid}/sessions/{sessionId:guid}/reservation")]
    public async Task<IActionResult> CancelReservation(
        Guid participantId, Guid sessionId)
    {
        var command = new CancelReservationCommand(participantId, sessionId);
        var cancelReservationResult = await serder.Send(command);
        return cancelReservationResult.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpPost("{participantId:guid}/sessions/{sessionId:guid}/reservation")]
    public async Task<IActionResult> CreateReservation(Guid participantId, Guid sessionId)
    {
        var command = new CreateReservationCommand(sessionId, participantId);
        var cancelReservationResult = await serder.Send(command);
        return cancelReservationResult.Match(
            _ => NoContent(),
            Problem
        );
    }
}