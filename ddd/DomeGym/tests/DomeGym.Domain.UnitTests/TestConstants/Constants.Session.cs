using DomeGym.Domain.Common.ValueObjects;
using FluentAssertions.Primitives;

namespace DomeGym.Domain.UnitTests.TestConstants;

public static partial class Constants
{
    public static class Session
    {
        public static Guid Id = Guid.NewGuid();
        public static readonly DateOnly Date = DateOnly.FromDateTime(DateTime.UtcNow);

        public static readonly TimeRange Time = new(
            start: TimeOnly.MinValue.AddHours(8),
            end: TimeOnly.MinValue.AddHours(9)
        );
        public const int MaxParticipants = 10;
    }
}