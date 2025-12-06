using GymManagment.Application.Gyms.Commands.CreateGym;
using TestCommon.TestContants;

namespace TestCommon.Gyms;

public static class GymCommandFactory
{
    public static CreateGymCommand CreateGymCommand(
        string name = Contants.Gym.Name,
        Guid? subscriptionId = null)
    {
        return new CreateGymCommand(name, subscriptionId ?? Contants.Subscriptions.Id);
    }
}