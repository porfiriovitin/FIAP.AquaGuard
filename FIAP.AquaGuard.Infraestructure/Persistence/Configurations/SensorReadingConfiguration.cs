using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Configurations;

public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
{
    public void Configure(EntityTypeBuilder<SensorReading> builder)
    {
        builder.ToTable("sensor_readings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WaterLevelCm).HasPrecision(12, 4);
        builder.Property(x => x.FlowRateM3s).HasPrecision(12, 4);
        builder.Property(x => x.RainfallMm).HasPrecision(12, 4);
        builder.Property(x => x.BatteryLevel).HasPrecision(5, 2);
        builder.Property(x => x.SignalStrength).HasPrecision(5, 2);
        builder.Property(x => x.MeasuredAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne(x => x.Sensor)
            .WithMany(x => x.SensorReadings)
            .HasForeignKey(x => x.SensorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
