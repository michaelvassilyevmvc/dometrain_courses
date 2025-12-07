using GymManagment.Application.Common.Behaviors;
using GymManagment.Application.Gyms.Commands.CreateGym;
using GymManagment.Domain.Gyms;
using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using TestCommon.Gyms;

namespace GymManagment.Application.UnitTests.Common.Behavior;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBehavior;
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _validationBehavior;

    public ValidationBehaviorTests()
    {
        

        // Создадим следующее поведение
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();
        
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
        
        // Создадим валидаток (mock)
        // Создадим поведение валидации
        _validationBehavior = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>(_mockValidator);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        // Создать запрос
        var createGymRequest = GymCommandFactory.CreateGymCommand();
        var gym = GymFactory.CreateGym();
        
        _mockValidator.ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        
        _mockNextBehavior.Invoke()
            .Returns(gym);

        // Act
        // Вызов поведения
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, CancellationToken.None);

        // Assert
        // Результат вызова поведения
        // Был результат следующего поведения
        result.IsError.Should()
            .BeFalse();
        result.Value.Should()
            .BeEquivalentTo(gym);
    }
    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsNotValid_ShouldReturnListErrors()
    {
        // Arrange
        // Создать запрос
        var createGymRequest = GymCommandFactory.CreateGymCommand();
        List<ValidationFailure> validationFailures = [new(propertyName: "foo", errorMessage: "bad foo")];
        
        _mockValidator.ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        
        ;

        // Act
        // Вызов поведения
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, CancellationToken.None);

        // Assert

        result.IsError.Should()
            .BeTrue();
        result.FirstError.Code.Should()
            .Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }
}