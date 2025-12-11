using DomeGym.Application.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IGymsRepository _gymsRepository;

    public DeleteRoomCommandHandler(IRoomsRepository roomsRepository, IGymsRepository gymsRepository)
    {
        _roomsRepository = roomsRepository;
        _gymsRepository = gymsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);
        if (gym is null)
        {
            return Error.NotFound("Gym not found");
        }

        if (!gym.HasRoom(command.RoomId))
        {
            return Error.NotFound("Room not found");
        }

        var room = await _roomsRepository.GetByIdAsync(command.GymId);
        if (room is null)
        {
            return Error.NotFound("Room not found");
        }

        var removeGymResult = gym.RemoveRoom(room);
        if (removeGymResult.IsError)
        {
            return removeGymResult.Errors;
        }

        await _gymsRepository.UpdateAsync(gym);
        return Result.Deleted;
    }
}