using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Queries.ListRooms;

public class ListRoomsQueryHandler: IRequestHandler<ListRoomsQuery, ErrorOr<List<Room>>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public ListRoomsQueryHandler(IGymsRepository gymsRepository, IRoomsRepository roomsRepository)
    {
        _gymsRepository = gymsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<List<Room>>> Handle(ListRoomsQuery query, CancellationToken cancellationToken)
    {
        if (!await _gymsRepository.ExistsAsync(query.GymId))
        {
            return Error.NotFound("Gym not found");
        }
        
        return await _roomsRepository.ListByGymIdAsync(query.GymId);
    }
}