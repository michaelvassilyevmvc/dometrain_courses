using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using MediatR;

namespace GymManagment.Application.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler: IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(IGymsRepository gymsRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _unitOfWork = unitOfWork;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        // проверка на существование зала
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);
        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        
        // проверка на существование комнаты
        
        if (!gym.HasRoom(command.RoomId))
        {
            return Error.NotFound(description: "Room not found");
        }

        // удаление комнаты
        gym.RemoveRoom(command.RoomId);

        // обновление локального хранилища и сохранение в БД
        await _gymsRepository.UpdateAsync(gym);
        await _unitOfWork.CommitChangesAsync();

        // вывод
        return Result.Deleted;
    }
}