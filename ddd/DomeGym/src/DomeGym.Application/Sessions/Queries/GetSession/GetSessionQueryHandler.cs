using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Sessions.Queries.GetSession;

public class GetSessionQueryHandler:IRequestHandler<GetSessionQuery, ErrorOr<Session>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IRoomsRepository _roomsRepository;

    public GetSessionQueryHandler(ISessionRepository sessionRepository, IRoomsRepository roomsRepository)
    {
        _sessionRepository = sessionRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<Session>> Handle(GetSessionQuery query, CancellationToken cancellationToken)
    {
        
        var room = await _roomsRepository.GetByIdAsync(query.RoomId);
        if (room is null)
        {
            return Error.NotFound("Room not found");
        }
        
        if (!room.HasSession(query.SessionId))
        {
            return Error.NotFound("Session not found");
        }

        var session = await _sessionRepository.GetByIdAsync(query.SessionId);
        if (session is null)
        {
            return Error.NotFound("Session not found");
        }

        return session;
    }
}