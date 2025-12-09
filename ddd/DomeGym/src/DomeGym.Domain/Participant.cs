using ErrorOr;

namespace DomeGym.Domain;

public class Participant
{
    private readonly Schedule _schedule = Schedule.Empty();

    public Guid Id { get; }

    private readonly Guid _userId;
    private readonly List<Guid> _sessionId = new();

    public Participant(Guid userId, Guid? id = null)
    {
        _userId = userId;
        Id = id ?? Guid.NewGuid();
    }

    public ErrorOr<Success> AddToSchedule(Session session)
    {
        if (_sessionId.Contains(item: session.Id))
        {
            return Error.Conflict(description: "Session already exists in participant's schedule");
        }

        var bookTimeSlotResult = _schedule.BookTimeSlot(date: session.Date, time: session.Time);
        if (bookTimeSlotResult.IsError)
        {
            return bookTimeSlotResult.FirstError.Type == ErrorType.Conflict
                ? ParticipantErrors.CannotHaveTwoOrMoreOverlappingSessions
                : bookTimeSlotResult.Errors;
        }

        _sessionId.Add(item: session.Id);
        return Result.Success;
    }
}