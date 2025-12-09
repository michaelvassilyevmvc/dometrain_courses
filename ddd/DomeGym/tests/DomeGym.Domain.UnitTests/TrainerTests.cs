using DomeGym.Domain.UnitTests.TestConstants;
using DomeGym.Domain.UnitTests.TestUtils.Common;
using DomeGym.Domain.UnitTests.TestUtils.Sessions;
using DomeGym.Domain.UnitTests.TestUtils.Trainers;
using FluentAssertions;

namespace DomeGym.Domain.UnitTests;

public class TrainerTests
{
    [Theory]
    [InlineData(1, 3, 1, 3)]
    [InlineData(1, 3, 2, 3)]
    [InlineData(1, 3, 2, 4)]
    [InlineData(1, 3, 0, 2)]
    public void AddSessionToSchedule_WhenSessionOverlapsWithAnotherSession_ShouldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2
    )
    {
        // Arrange
        var trainer = TrainerFactory.CreateTrainer();
        var session1 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHour: startHourSession1, endHour: endHourSession1),
            id: Guid.NewGuid()
        );
        var session2 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHour: startHourSession2, endHour: endHourSession2),
            id: Guid.NewGuid()
        );
        
        // Act
        var addSessionn1Result = trainer.AddSessionToSchedule(session1);
        var addSessionn2Result = trainer.AddSessionToSchedule(session2);
        
        // Assert
        addSessionn1Result.IsError.Should().BeFalse();
        addSessionn2Result.IsError.Should().BeTrue();
        addSessionn2Result.FirstError.Should().Be(TrainerErrors.CannotHaveTwoOrMoreOverlappingSessions);

    }
}