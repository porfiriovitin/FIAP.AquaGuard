using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class RiskAnalysis
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public decimal? ForecastRain24hMm { get; set; }
    public decimal? AccumulatedRain7dMm { get; set; }
    public decimal? CurrentOrForecastFlow { get; set; }
    public decimal? MaxFlowNextDays { get; set; }
    public decimal? TerrainElevationMeters { get; set; }
    public decimal? DetectedWaterPercentage { get; set; }
    public int RiskScore { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public string? AlertMessage { get; set; }
    public DateTimeOffset AnalyzedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public City City { get; set; } = null!;
    public ICollection<RiskAnalysisSensorReading> RiskAnalysisSensorReadings { get; set; } = new List<RiskAnalysisSensorReading>();
    public ICollection<RiskDataSource> RiskDataSources { get; set; } = new List<RiskDataSource>();
}
