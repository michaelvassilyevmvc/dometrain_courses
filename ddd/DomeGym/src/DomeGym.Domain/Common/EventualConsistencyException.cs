using ErrorOr;

namespace DomeGym.Domain.Common;

public class EventualConsistencyException : Exception
{
    public EventualConsistencyException(
        Error eventualConsistencyError,
        List<Error>? underlyingErrors = null)
        : base(message: eventualConsistencyError.Description)
    {
        EventualConsistencyError = eventualConsistencyError;
        UnderlyingErrors = underlyingErrors ?? new ();
    }

    public Error EventualConsistencyError { get; }
    public List<Error> UnderlyingErrors { get; }
}