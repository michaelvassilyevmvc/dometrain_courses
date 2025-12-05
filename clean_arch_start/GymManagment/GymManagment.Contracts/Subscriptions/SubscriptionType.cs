using System.Text.Json.Serialization;

namespace GymManagment.Contracts.Subscriptions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Free,
    Starter, 
    Pro
}