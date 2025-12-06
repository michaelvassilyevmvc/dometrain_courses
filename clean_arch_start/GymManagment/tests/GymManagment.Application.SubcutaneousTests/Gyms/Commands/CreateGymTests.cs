using ErrorOr;
using FluentAssertions;
using GymManagment.Application.SubcutaneousTests.Common;
using GymManagment.Domain.Subscriptions;
using MediatR;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagment.Application.SubcutaneousTests.Gyms.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateGymTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task CreateGym_WhenValidCommand_ShouldCreateGym()
    {
        // Arrange
        var subscription = await CreateSubscription();
        // Создать CreateGymCommand
        var createGymCommand = GymCommandFactory.CreateGymCommand(subscriptionId: subscription.Id);
        // Act
        // Отправить команду CreateGymCommand в MediatR
        var createGymResult = await _mediator.Send(createGymCommand);

        // Assert
        // В результате получается зал, соответствующий деталям команды создания зала
        createGymResult.IsError.Should()
            .BeFalse();
        createGymResult.Value.SubscriptionId.Should()
            .Be(subscription.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(200)]
    public async Task CreateGym_WhenCommandContainsInvalidData_ShouldReturnValidation(int gymNameLength)
    {
        string gymName = new string('a', gymNameLength);
        var createGymCommand = GymCommandFactory.CreateGymCommand(gymName);
        var result = await _mediator.Send(createGymCommand);
        ;
        result.IsError.Should()
            .BeTrue();
        result.FirstError.Code.Should()
            .Be("Name");
    }

  
    private async Task<Subscription> CreateSubscription()
    {
        // Создать subscription
        // 1. Создать CreateSubscriptionCommand
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateSubscriptionCommand();
        // 2. Отправить в MediatR
        var result = await _mediator.Send(createSubscriptionCommand);
        // 3. Убедиться, что он был создан успешно
        result.IsError.Should()
            .BeFalse();
        return result.Value;
    }
}