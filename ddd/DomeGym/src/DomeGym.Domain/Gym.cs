namespace DomeGym.Domain;
using ErrorOr;

public class Gym
{
    private readonly Guid _subscriptionId;
    private readonly int _maxRooms;
    private readonly List<Guid> _roomIds = new();
    
    public Guid Id { get; }

    public Gym(
        int maxRooms,
        Guid subscriptionId,
        Guid? id = null
    )
    {
        _maxRooms = maxRooms;
        _subscriptionId = subscriptionId;
        Id = id ?? Guid.NewGuid();
    }
    
    public ErrorOr<Success> AddRoom(Room room)
    {
        // CanTest: на добавление уже существующего зала
        if (_roomIds.Contains(room.Id))
        {
            return Error.Conflict("Room already exists in gym");
        }
        
        // CanTest: на возможность добавить больше залов чем разрешено по подписке
        if (_roomIds.Count + 1 > _maxRooms)
        {
            return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
        }
        
        _roomIds.Add(room.Id);
        return Result.Success;
    }
    
}
