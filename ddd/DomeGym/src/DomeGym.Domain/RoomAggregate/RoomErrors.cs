using ErrorOr;

namespace DomeGym.Domain.RoomAggregate;

public static class RoomErrors
{
    public static readonly Error CannotHaveMoreSessionThanSubscriptionAllows = Error.Validation(
        code: "Room.CannotHaveMoreSessionThanSubscriptionAllows",
        description: "A room cannot have more scheduled sessions than the subscription allows"
    );
    
    public static readonly Error CannotHaveTwoOrMoreOverlappingSessions = Error.Validation(
        code: "Room.CannotHaveTwoOrMoreOverlappingSessions",
        description: "A room cannot have two or more overlapping sessions"
    );
}