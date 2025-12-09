using ErrorOr;

namespace DomeGym.Domain;

public static class ParticipantErrors
{
    public static readonly Error CannotHaveTwoOrMoreOverlappingSessions = Error.Validation(
        code: "Participant.CannotHaveTwoOrMoreOverlappingSessions",
        description: "A participant cannot have two or more overlapping sessions"
    );
}