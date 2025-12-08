namespace DomeGym.Domain;

public class Participant
{
    private readonly Guid _id;

    public Guid Id => _id;

    private readonly Guid _userId;
    private readonly List<Guid> _sessionId = new();

    public Participant(Guid userId, Guid? id = null)
    {
        _userId = userId;
        _id = id ?? Guid.NewGuid();
    }
}