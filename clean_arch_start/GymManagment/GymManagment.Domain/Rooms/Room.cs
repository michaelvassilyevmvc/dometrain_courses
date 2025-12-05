namespace GymManagment.Domain.Rooms;

public class Room
{
    public Guid Id { get; private set; }
    public string Name { get; } = null!;

    public Guid GymId { get; }
    public int MaxDailySessions { get; }

    public Room(
        string name,
        Guid gymId,
        int maxDailySessions,
        Guid? id = null
    )
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        GymId = gymId;
        MaxDailySessions = maxDailySessions;
    }
}