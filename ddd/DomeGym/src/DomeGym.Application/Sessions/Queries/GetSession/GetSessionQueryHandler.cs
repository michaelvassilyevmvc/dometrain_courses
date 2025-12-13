using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Sessions.Queries.GetSession;

public class GetSessionQueryHandler:IRequestHandler<GetSessionQuery, ErrorOr<Session>>
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public GetSessionQueryHandler(ISessionsRepository sessionsRepository, IRoomsRepository roomsRepository)
    {
        _sessionsRepository = sessionsRepository;
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

        var session = await _sessionsRepository.GetByIdAsync(query.SessionId);
        if (session is null)
        {
            return Error.NotFound("Session not found");
        }

        return session;
    }
}