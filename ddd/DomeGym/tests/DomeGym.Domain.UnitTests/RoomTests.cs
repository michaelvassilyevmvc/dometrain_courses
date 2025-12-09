using DomeGym.Domain.RoomAggregate;
using DomeGym.Domain.UnitTests.TestConstants;
using DomeGym.Domain.UnitTests.TestUtils.Common;
using DomeGym.Domain.UnitTests.TestUtils.Rooms;
using DomeGym.Domain.UnitTests.TestUtils.Sessions;
using FluentAssertions;

namespace DomeGym.Domain.UnitTests;

public class RoomTests
{
    [Fact]
    public void ScheduleSession_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        var room = RoomFactory.CreateRoom(1);
        
        var session1 = SessionFactory.CreateSession(id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(id: Guid.NewGuid());
        
        // Act
        var scheduleSession1Result = room.ScheduleSession(session: session1);
        var scheduleSession2Result = room.ScheduleSession(session: session2);
        
        // Assert
        scheduleSession1Result.IsError.Should()
            .BeFalse();
        scheduleSession2Result.IsError.Should()
            .BeTrue();
        scheduleSession2Result.FirstError.Should()
            .Be(RoomErrors.CannotHaveMoreSessionThanSubscriptionAllows);
    }

    [Theory]
    [InlineData(1, 3, 1, 3)]

    public void ScheduleSession_WhenSessionOverlapsWithAnotherSession_ShoulldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2
    )
    {
        // Arrange
        var room = RoomFactory.CreateRoom(2);
        var session1 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHour: startHourSession1,
                endHour: endHourSession1),
            id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHour: startHourSession2,
                endHour: endHourSession2),
            id: Guid.NewGuid());
        // Act

        var scheduleSession1Result = room.ScheduleSession(session: session1);
        var scheduleSession2Result = room.ScheduleSession(session: session2);
        
        // Assert
        scheduleSession1Result.IsError.Should()
            .BeFalse();
        scheduleSession2Result.IsError.Should()
            .BeTrue();
        scheduleSession2Result.FirstError.Should()
            .Be(RoomErrors.CannotHaveTwoOrMoreOverlappingSessions);
    }
}