using DomeGym.Domain.Common.Entities;
using DomeGym.Domain.Common.ValueObjects;
using DomeGym.Domain.TrainerAggregate;
using DomeGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class TrainerConfigurations: IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(trainer => trainer.Id);
        builder.Property(trainer => trainer.Id)
            .ValueGeneratedNever();
        builder.Property<List<Guid>>("_sessionIds").HasListOfIdsConverter().HasColumnName("SessionIds");
        builder.Property(trainer => trainer.UserId);
        builder.OwnsOne<Schedule>("_schedule", sb =>
        {
            sb.Property<Dictionary<DateOnly, List<TimeRange>>>("_calendar")
                .HasColumnName("ScheduleCalendar")
                .HasValueJsonConverter();
            sb.Property(s => s.Id)
                .HasColumnName("ScheduleId");
        });
    }
}