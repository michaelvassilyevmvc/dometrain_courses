using DomeGym.Domain.UnitTests.TestUtils.Participants;
using DomeGym.Domain.UnitTests.TestUtils.Sessions;
using FluentAssertions;

namespace DomeGym.Domain.UnitTests;

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
        session.ReserveSpot(participant1);
        var action = () => session.ReserveSpot(participant2);
        
        // Assert
        // participant 2 revervation failed
        action.Should()
            .ThrowExactly<Exception>();
    }
}