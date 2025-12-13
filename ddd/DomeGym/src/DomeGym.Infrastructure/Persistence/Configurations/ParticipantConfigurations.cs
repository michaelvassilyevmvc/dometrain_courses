using DomeGym.Domain.Common.Entities;
using DomeGym.Domain.Common.ValueObjects;
using DomeGym.Domain.ParticipantAggregate;
using DomeGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class ParticipantConfigurations : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(participant => participant.Id);
        builder.Property(participant => participant.Id)
            .ValueGeneratedNever();
        builder.OwnsOne<Schedule>("_schedule", sb =>
        {
            sb.Property<Dictionary<DateOnly, List<TimeRange>>>("_calendar")
                .HasColumnName("ScheduleCalendar")
                .HasValueJsonConverter();
            sb.Property(s => s.Id)
                .HasColumnName("ScheduleId");
        });

        builder.Property<List<Guid>>("_sessionIds")
            .HasColumnName("SessionIds")
            .HasListOfIdsConverter();
        builder.Property(g => g.UserId);
    }
}