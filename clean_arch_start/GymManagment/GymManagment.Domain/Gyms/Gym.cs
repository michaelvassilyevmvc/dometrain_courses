using ErrorOr;
using GymManagment.Domain.Rooms;
using Throw;

namespace GymManagment.Domain.Gyms;

public class Gym
{
    private Gym()
    {
    }

    public Gym(
        string name,
        int maxRooms,
        Guid subscriptionId,
        Guid? id = null
    )
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        SubscriptionId = subscriptionId;
        _maxRooms = maxRooms;
    }

    public Guid Id { get; private set; }
    private readonly List<Guid> _roomIds = [];
    private readonly List<Guid> _trainerIds = [];

    private readonly int _maxRooms;

    public string Name { get; init; } = null!;
    public Guid SubscriptionId { get; init; }

    public bool HasRoom(Guid roomId) => _roomIds.Contains(roomId);

    public ErrorOr<Success> AddRoom(Room room)
    {
        _roomIds.Throw()
            .IfContains(element: room.Id);

        _roomIds.Add(room.Id);
        return Result.Success;
    }

    public void RemoveRoom(Guid roomId)
    {
        _roomIds.Remove(roomId);
    }

    public bool HasTrainer(Guid trainerId) => _trainerIds.Contains(trainerId);

    public ErrorOr<Success> AddTrainer(Guid trainerId)
    {
        if (_trainerIds.Contains(trainerId))
        {
            return Error.Conflict(description: "Trainer already added to gym");
        }

        _trainerIds.Add(trainerId);
        return Result.Success;
    }
}