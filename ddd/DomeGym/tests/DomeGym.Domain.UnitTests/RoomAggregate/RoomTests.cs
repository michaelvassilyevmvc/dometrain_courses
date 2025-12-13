using DomeGym.Domain.RoomAggregate;
using DomeGym.Domain.UnitTests.TestConstants;
using DomeGym.Domain.UnitTests.TestUtils.Common;
using DomeGym.Domain.UnitTests.TestUtils.Rooms;
using DomeGym.Domain.UnitTests.TestUtils.Sessions;
using FluentAssertions;

namespace DomeGym.Domain.UnitTests.RoomAggregate;

public class RoomTests
{
    [Fact]
    public void ScheduleSession_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        var room = RoomFactory.CreateRoom(maxDailySession:1);
        
        var firstDailySession = SessionFactory.CreateSession(date: Constants.Session.Date,id: Guid.NewGuid());
        var secondDailySession = SessionFactory.CreateSession(date: Constants.Session.Date,id: Guid.NewGuid());
        var sessionOnAnotherDay = SessionFactory.CreateSession(date: Constants.Session.Date.AddDays(1),id: Guid.NewGuid());
        
        
        // Act
        var scheduleFirstSessionResult = room.ScheduleSession(session: firstDailySession);
        var scheduleSecondSessionResult = room.ScheduleSession(session: secondDailySession);
        var scheduleSessionOnAnotherDayResult = room.ScheduleSession(session: sessionOnAnotherDay);
        
        // Assert
        scheduleFirstSessionResult.IsError.Should()
            .BeFalse();
        scheduleSessionOnAnotherDayResult.IsError.Should()
            .BeFalse();
        scheduleSecondSessionResult.IsError.Should()
            .BeTrue();
        scheduleSecondSessionResult.FirstError.Should()
            .Be(RoomErrors.CannotHaveMoreSessionThanSubscriptionAllows);
    }

    [Theory]
    [InlineData(1, 3, 1, 3)] // exact overlap
    [InlineData(1, 3, 2, 3)] // second session inside first session
    [InlineData(1, 3, 2, 4)] // second session ends after session, but overlaps
    [InlineData(1, 3, 0, 2)]

    public void ScheduleSession_WhenSessionOverlapsWithAnotherSession_ShoulldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2
    )
    {
        // Arrange
        var room = RoomFactory.CreateRoom(maxDailySession:2);
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