using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infraestructure.Persistence.Configurations;

public class RiskAnalysisSensorReadingConfiguration : IEntityTypeConfiguration<RiskAnalysisSensorReading>
{
    public void Configure(EntityTypeBuilder<RiskAnalysisSensorReading> builder)
    {
        builder.ToTable("risk_analysis_sensor_readings");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.RiskAnalysis)
            .WithMany(x => x.RiskAnalysisSensorReadings)
            .HasForeignKey(x => x.RiskAnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SensorReading)
            .WithMany(x => x.RiskAnalysisSensorReadings)
            .HasForeignKey(x => x.SensorReadingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.RiskAnalysisId, x.SensorReadingId }).IsUnique();
    }
}
