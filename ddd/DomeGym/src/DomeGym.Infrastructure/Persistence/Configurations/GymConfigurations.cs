using DomeGym.Domain.GymAggregate;
using DomeGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeGym.Infrastructure.Persistence.Configurations;

public class GymConfigurations: IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.HasKey(gym => gym.Id);
        builder.Property(gym => gym.Id).ValueGeneratedNever();
        builder.Property(gym => gym.Name);
        builder.Property(gym => gym.SubscriptionId);
        builder.Property("_maxRooms").HasColumnName("MaxRooms");
        builder.Property<List<Guid>>("_roomIds").HasColumnName("RoomIds").HasListOfIdsConverter();
        builder.Property<List<Guid>>("_trainerIds").HasColumnName("TrainerIds").HasListOfIdsConverter();

    }
}