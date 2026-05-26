using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlaceName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Latitude).HasPrecision(10, 6);
        builder.Property(x => x.Longitude).HasPrecision(10, 6);
        builder.Property(x => x.SensorType).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.InstalledAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne(x => x.City)
            .WithMany(x => x.Sensors)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
