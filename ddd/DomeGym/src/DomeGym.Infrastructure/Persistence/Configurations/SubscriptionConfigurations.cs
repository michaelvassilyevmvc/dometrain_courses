using DomeGym.Domain.SubscriptionAggregate;
using DomeGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class SubscriptionConfigurations: IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(subscription => subscription.Id);
        builder.Property(subscription => subscription.Id)
            .ValueGeneratedNever();
        builder.Property("_maxGyms")
            .HasColumnName("MaxGyms");
        builder.Property("_adminId")
            .HasColumnName("AdminId");
        builder.Property(s => s.SubscriptionType)
            .HasConversion(subscriptionType => subscriptionType.Value,
                value => SubscriptionType.FromValue(value));
        builder.Property<List<Guid>>("_gymIds")
            .HasColumnName("GymIds")
            .HasListOfIdsConverter();
    }
}