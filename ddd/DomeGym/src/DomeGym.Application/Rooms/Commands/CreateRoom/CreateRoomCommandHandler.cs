using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler: IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateRoomCommandHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);
        if(gym is null)
        {
            return Error.NotFound("Gym not found");
        }

        var subscription = await _subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);
        if (subscription is null)
        {
            return Error.Unexpected("Subscription not found");
        }

        var room = new Room(
            name: command.RoomName,
            maxDailySessions: subscription.GetMaxDailySessions(),
            gymId: gym.Id
        );

        var addGymResult = gym.AddRoom(room);
        if(addGymResult.IsError)
        {
            return addGymResult.Errors;
        }
        
        await _gymsRepository.UpdateAsync(gym);
        return room;
    }
}