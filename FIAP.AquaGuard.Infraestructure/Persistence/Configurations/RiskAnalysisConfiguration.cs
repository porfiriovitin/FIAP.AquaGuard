using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infraestructure.Persistence.Configurations;

public class RiskAnalysisConfiguration : IEntityTypeConfiguration<RiskAnalysis>
{
    public void Configure(EntityTypeBuilder<RiskAnalysis> builder)
    {
        builder.ToTable("risk_analyses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ForecastRain24hMm).HasPrecision(12, 4);
        builder.Property(x => x.AccumulatedRain7dMm).HasPrecision(12, 4);
        builder.Property(x => x.CurrentOrForecastFlow).HasPrecision(12, 4);
        builder.Property(x => x.MaxFlowNextDays).HasPrecision(12, 4);
        builder.Property(x => x.TerrainElevationMeters).HasPrecision(12, 4);
        builder.Property(x => x.DetectedWaterPercentage).HasPrecision(5, 2);
        builder.Property(x => x.RiskScore).IsRequired();
        builder.Property(x => x.RiskLevel).IsRequired();
        builder.Property(x => x.AlertMessage).HasMaxLength(1000);
        builder.Property(x => x.AnalyzedAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne(x => x.City)
            .WithMany(x => x.RiskAnalyses)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
