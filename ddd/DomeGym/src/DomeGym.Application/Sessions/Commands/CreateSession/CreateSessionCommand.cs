using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid RoomId,
    string Name,
    string Description,
    int MaxParticipants,
    DateTime StartDateTime,
    DateTime EndDateTime,
    Guid TrainerId,
    List<SessionCategory> Categories) : IRequest<ErrorOr<Session>>;