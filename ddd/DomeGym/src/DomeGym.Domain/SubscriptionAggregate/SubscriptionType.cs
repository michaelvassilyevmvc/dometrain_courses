using Ardalis.SmartEnum;

namespace DomeGym.Domain.SubscriptionAggregate;

public class SubscriptionType: SmartEnum<SubscriptionType>
{
    public static readonly SubscriptionType Free = new SubscriptionType(nameof(Free), 0);
    public static readonly SubscriptionType Starter = new SubscriptionType(nameof(Starter), 0);
    public static readonly SubscriptionType Pro = new SubscriptionType(nameof(Pro), 0);
    
    public SubscriptionType(string name, int value) : base(name, value)
    {
    }
}