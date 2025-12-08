using DomeGym.Domain.UnitTests.TestConstants;
using DomeGym.Domain.UnitTests.TestUtils.Participants;
using DomeGym.Domain.UnitTests.TestUtils.Services;
using DomeGym.Domain.UnitTests.TestUtils.Sessions;
using FluentAssertions;
using ErrorOr;

namespace DomeGym.Domain.UnitTests;

// - Сеанс не может содержать больше максимального количества участников.
// - Бронирование не может быть отменено бесплатно менее чем за 24 часа до начала сеанса.

public class SessionTest
{
    [Fact]
    public void ReserveSpot_WhenNoMoreRoom_ShouldFailReservation()
    {
        // Arrange
        // Create Session with a maximum participants of 1
        var session = SessionFactory.CreateSession(maxParticipants: 1);
        var participant1 = ParticipantFactory.CreateParticipant(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid());
        var participant2 = ParticipantFactory.CreateParticipant(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid());
        // Create 2 participants
        
        // Act
        // Add participant 1
        // Add participant 2
        var reserveParticipant1Result1 =  session.ReserveSpot(participant1);
        var reserveParticipant1Result2 =  session.ReserveSpot(participant2);
        
        // Assert
        // participant 2 revervation failed
        reserveParticipant1Result1.IsError.Should().BeFalse();
        reserveParticipant1Result2.IsError.Should().BeTrue();
        reserveParticipant1Result2.FirstError.Should().Be(SessionErrors.CannotHaveMoreReservationsThanParticipants);
        
    }

    [Fact]
    public void CancelReservation_WhenCancellationIsTooCloseToSession_ShouldFailCancellation()
    {
        // Arrange
        // Create a session
        
        var session = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: Constants.Session.Time);
        
        // Create a participant
        var participant = ParticipantFactory.CreateParticipant(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid());
        
        var cancellationDateTime = Constants.Session.Date.ToDateTime(TimeOnly.MinValue);
        
        // Act
        // Cancel the reservation less than 24 hours before the session starts
        // Reserve a spot for the participant in the session
        var reserveSpotResult = session.ReserveSpot(participant);
        var cancelReservationResult = session.CancelReservation(
            participant, 
            new TestDateTimeProvider(fixedDateTime: cancellationDateTime)
        );

        // Assert
        // Cancellation failed
        reserveSpotResult.IsError.Should().BeFalse();
        cancelReservationResult.IsError.Should().BeTrue();
        cancelReservationResult.FirstError.Should().Be(SessionErrors.CannotCancelReservationTooCloseToSession);

    }
}