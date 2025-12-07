using ErrorOr;
using FluentAssertions;
using GymManagment.Domain.Subscriptions;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagment.Domain.UnitTests.Subscriptions;

public class SubscriptionTests
{
    // Когда- что-то - должно произойти
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        // Создать subscription
        var subscription = SubscriptionFactory.CreateSubscription();

        // Создать максимальное число залов + 1
        var gyms = Enumerable.Range(0, subscription.GetMaxRooms() + 1)
            .Select(x => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();


        // Act
        // Добавить все залы
        var addGymResults = gyms.ConvertAll(subscription.AddGym);

        // Assert
        // Добавить все залы - succeeded, последний failed
        var allButLastGymResults = addGymResults[..^1];
        allButLastGymResults.Should().AllSatisfy(addGymResult => addGymResult.Value.Should().Be(Result.Success));
        
        var lastAddGymResult = addGymResults.Last();
        lastAddGymResult.IsError.Should()
            .BeTrue();

        lastAddGymResult.FirstError.Should()
            .Be(SubscriptionErrors.CannotHaveMoreGymsThanTheSubscriptionAllows);
    }
}