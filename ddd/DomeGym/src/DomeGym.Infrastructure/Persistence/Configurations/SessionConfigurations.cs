using DomeGym.Domain.SessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class SessionConfigurations: IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(session => session.Id);
        builder.Property(session => session.Id)
            .ValueGeneratedNever();
        builder.Property(session => session.TrainerId);
        builder.OwnsMany<Reservation>("_reservations", rb =>
        {
            rb.ToTable("SessionReservations");
            rb.HasKey(r => r.Id);
            rb.Property(r => r.Id)
                .ValueGeneratedNever();
            rb.WithOwner()
                .HasForeignKey("SessionId");
            rb.Property(r => r.ParticipantId);
        });

        builder.OwnsMany(s => s.Categories, cb =>
        {
            cb.ToTable("SessionCategories");
            cb.WithOwner()
                .HasForeignKey("SessionId");
            cb.HasKey("Id");
            cb.Property(c => c.Name);
            cb.Property(c => c.Value);
        }).Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(s => s.MaxParticipants);
        builder.Property(s => s.Date);
        builder.OwnsOne(s => s.Time);
        builder.Property(s => s.Name);
        builder.Property(s => s.Description);
        builder.Property(s => s.RoomId);
    }
}