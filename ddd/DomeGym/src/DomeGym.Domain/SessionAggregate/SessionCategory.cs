using Ardalis.SmartEnum;

namespace DomeGym.Domain.SessionAggregate;

public class SessionCategory: SmartEnum<SessionCategory>
{
    public static readonly SessionCategory Kickboxing = new SessionCategory(nameof(Kickboxing), 0);
    public static readonly SessionCategory Functional = new SessionCategory(nameof(Functional), 1);
    public static readonly SessionCategory Zoomba = new SessionCategory(nameof(Zoomba), 2);
    public static readonly SessionCategory Pilates = new SessionCategory(nameof(Pilates), 3);
    public static readonly SessionCategory Yoga = new SessionCategory(nameof(Yoga), 4);
    
    public SessionCategory(string name, int value) : base(name, value)
    {
    }
}