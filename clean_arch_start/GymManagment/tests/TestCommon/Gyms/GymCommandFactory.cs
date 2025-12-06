using GymManagment.Application.Gyms.Commands.CreateGym;
using TestCommon.TestContants;

namespace TestCommon.Gyms;

public static class GymCommandFactory
{
    public static CreateGymCommand CreateGymCommand(
        string name = Constants.Gym.Name,
        Guid? subscriptionId = null)
    {
        return new CreateGymCommand(name, subscriptionId ?? Constants.Subscriptions.Id);
    }
}