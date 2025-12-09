using DomeGym.Domain.Common;
using DomeGym.Domain.RoomAggregate;
using ErrorOr;

namespace DomeGym.Domain.GymAggregate;

public class Gym: AggregateRoot
{
    private readonly Guid _subscriptionId;
    private readonly int _maxRooms;
    private readonly List<Guid> _roomIds = new();
    

    public Gym(
        int maxRooms,
        Guid subscriptionId,
        Guid? id = null
    ): base(id ?? Guid.NewGuid())
    {
        _maxRooms = maxRooms;
        _subscriptionId = subscriptionId;
    }
    
    public ErrorOr<Success> AddRoom(Room room)
    {
        // CanTest: на добавление уже существующего зала
        if (_roomIds.Contains(room.Id))
        {
            return Error.Conflict("Room already exists in gym");
        }
        
        // CanTest: на возможность добавить больше залов чем разрешено по подписке
        if (_roomIds.Count >= _maxRooms)
        {
            return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
        }
        
        _roomIds.Add(room.Id);
        return Result.Success;
    }
    
}
