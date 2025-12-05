using ErrorOr;
using GymManagment.Domain.Gyms;
using Throw;

namespace GymManagment.Domain.Subscriptions;

public class Subscription
{
    private readonly List<Guid> _gymIds = [];
    private readonly int _maxGyms;

    private Subscription()
    {
    }

    public Guid Id { get; private set; }
    public SubscriptionType SubscriptionType { get; private set; } = null!;
    public Guid AdminId { get; }

    public Subscription(
        SubscriptionType subscriptionType,
        Guid adminId,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        AdminId = adminId;
        SubscriptionType = subscriptionType;

        _maxGyms = GetMaxGyms();
    }

    public ErrorOr<Success> AddGym(Gym gym)
    {
        _gymIds.Throw()
            .IfContains(gym.Id);

        if (_gymIds.Count >= _maxGyms)
        {
            return SubscriptionErrors.CannotHaveMoreGymsThanTheSubscriptionAllows;
        }

        _gymIds.Add(gym.Id);
        return Result.Success;
    }

    public bool HasGym(Guid gymId) => _gymIds.Contains(gymId);

    public void RemoveGym(Guid gymId)
    {
        _gymIds.Throw()
            .IfContains(gymId);
        _gymIds.Remove(gymId);
    }

    public int GetMaxGyms() =>
        SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 1,
            nameof(SubscriptionType.Pro) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

    public int GetMaxRooms() =>
        SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 3,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new ArgumentOutOfRangeException()
        };

    public int GetMaxSessions() =>
        SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 4,
            nameof(SubscriptionType.Starter) => int.MaxValue,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new ArgumentOutOfRangeException()
        };
}