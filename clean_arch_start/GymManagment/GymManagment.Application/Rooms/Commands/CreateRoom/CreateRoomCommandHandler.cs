using GymManagment.Domain.Rooms;
using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;

namespace GymManagment.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateRoomCommandHandler(
        IGymsRepository gymsRepository,
        IUnitOfWork unitOfWork,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _unitOfWork = unitOfWork;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        // проверка на существоания зала
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);
        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }
        
        //проверка на существование подписки
        var subscription = await _subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);
        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }
        
        // создание комнаты
        var room = new Room(name: command.RoomName, gymId: command.GymId, maxDailySessions:subscription.GetMaxSessions());
        
        // добавление комнаты в зал
        var addRoomResult = gym.AddRoom(room);
        
        // проверка создалась ли комната
        if (addRoomResult.IsError)
        {
            return addRoomResult.Errors;
        }
        
        // обновление локального хранилища и сохранение в БД
        await _gymsRepository.UpdateAsync(gym);
        await _unitOfWork.CommitChangesAsync();
        // вывод
        return room;
    }
}