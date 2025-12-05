using GymManagment.Domain.Gyms;
using TestCommon.TestContants;

namespace TestCommon.Gyms;

public static class GymFactory
{
    public static Gym CreateGym(
        string name = Contants.Gym.Name,
        int maxRooms = Contants.Subscriptions.MaxRoomsFreeTier,
        Guid? id = null)
    {
        return new Gym(name, maxRooms, subscriptionId: Contants.Subscriptions.Id, id: id ?? Contants.Gym.Id);
    }
}