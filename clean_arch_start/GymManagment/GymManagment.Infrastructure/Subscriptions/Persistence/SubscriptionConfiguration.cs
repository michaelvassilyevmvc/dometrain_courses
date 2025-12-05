using GymManagment.Domain.Subscriptions;
using GymManagment.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagment.Infrastructure.Subscriptions.Persistence;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property("_maxGyms")
            .HasColumnName("MaxGyms");

        builder.Property(s => s.AdminId);
        builder.Property(s => s.SubscriptionType)
            .HasConversion(
                subscriptionType => subscriptionType.Value,
                value => SubscriptionType.FromValue(value)
            );
        builder.Property<List<Guid>>("_gymIds")
            .HasColumnName("GymIds")
            .HasListOfIdsConverter();
    }
}