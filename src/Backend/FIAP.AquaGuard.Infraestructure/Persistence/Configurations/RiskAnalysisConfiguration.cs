using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Configurations;

public class RiskAnalysisConfiguration : IEntityTypeConfiguration<RiskAnalysis>
{
    public void Configure(EntityTypeBuilder<RiskAnalysis> builder)
    {
        builder.ToTable("risk_analyses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ForecastRain24hMm).HasColumnName("forecast_rain_24h_mm").HasPrecision(12, 4);
        builder.Property(x => x.AccumulatedRain7dMm).HasColumnName("accumulated_rain_7d_mm").HasPrecision(12, 4);
        builder.Property(x => x.CurrentOrForecastFlow).HasColumnName("current_or_forecast_flow").HasPrecision(12, 4);
        builder.Property(x => x.MaxFlowNextDays).HasColumnName("max_flow_next_days").HasPrecision(12, 4);
        builder.Property(x => x.TerrainElevationMeters).HasColumnName("terrain_elevation_meters").HasPrecision(12, 4);
        builder.Property(x => x.DetectedWaterPercentage).HasColumnName("detected_water_percentage").HasPrecision(5, 2);
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
