using DomeGym.Domain.AdminAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class AdminConfigurations: IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(admin => admin.Id);
        builder.Property(admin => admin.Id).ValueGeneratedNever();
        builder.Property(admin => admin.UserId);
        builder.Property(admin => admin.SubscriptionId);

    }
}