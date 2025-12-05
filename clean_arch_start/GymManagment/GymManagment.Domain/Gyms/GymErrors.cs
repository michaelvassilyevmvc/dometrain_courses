using ErrorOr;

namespace GymManagment.Domain.Gyms;

public static class GymErrors
{
    public static readonly Error CannotHaveMoreRoomsThanSubscriptionAllows = Error.Validation(
        code: "Room.CannotHaveMoreRoomsThanSubscriptionAllows",
        description: "A gym cannot have more rooms than the subscription allows"
    );
}