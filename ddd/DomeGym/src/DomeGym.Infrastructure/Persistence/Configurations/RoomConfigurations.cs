using DomeGym.Domain.Common.Entities;
using DomeGym.Domain.Common.ValueObjects;
using DomeGym.Domain.RoomAggregate;
using DomeGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class RoomConfigurations: IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(room => room.Id);
        builder.Property(room => room.Id)
            .ValueGeneratedNever();
        builder.Property("_maxDailySessions")
            .HasColumnName("MaxDailySessions");
        builder.Property<Dictionary<DateOnly, List<Guid>>>("_sessionIdsByDate")
            .HasColumnName("SessionIdsByDate")
            .HasValueJsonConverter();

        builder.OwnsOne<Schedule>("_schedule", sb =>
        {
            sb.Property<Dictionary<DateOnly, List<TimeRange>>>("_calendar")
                .HasColumnName("Calendar")
                .HasValueJsonConverter();

            sb.Property(s => s.Id)
                .HasColumnName("ScheduleId");
        });

        builder.Property(r => r.Name);
        builder.Property(r => r.GymId);
    }
}