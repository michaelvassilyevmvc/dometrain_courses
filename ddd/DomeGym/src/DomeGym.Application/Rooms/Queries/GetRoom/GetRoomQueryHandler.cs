using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, ErrorOr<Room>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public GetRoomQueryHandler(IGymsRepository gymsRepository, IRoomsRepository roomsRepository)
    {
        _gymsRepository = gymsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<Room>> Handle(GetRoomQuery query, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(query.GymId);
        if (gym is null)
        {
            return Error.NotFound("Gym not found");
        }

        if (!gym.HasRoom(query.RoomId))
        {
            return Error.NotFound("Room not found");
        }

        if (await _roomsRepository.GetByIdAsync(query.RoomId) is not Room room)
        {
            return Error.NotFound("Room not found");
        }

        return room;
    }
}