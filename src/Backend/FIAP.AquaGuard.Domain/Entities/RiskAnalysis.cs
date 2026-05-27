using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class RiskAnalysis
{
    public Guid Id { get; private set; }
    public Guid CityId { get; private set; }
    public decimal? ForecastRain24hMm { get; private set; }
    public decimal? AccumulatedRain7dMm { get; private set; }
    public decimal? CurrentOrForecastFlow { get; private set; }
    public decimal? MaxFlowNextDays { get; private set; }
    public decimal? TerrainElevationMeters { get; private set; }
    public decimal? DetectedWaterPercentage { get; private set; }
    public int RiskScore { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public string? AlertMessage { get; private set; }
    public DateTimeOffset AnalyzedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public City City { get; private set; } = null!;

    private readonly List<RiskAnalysisSensorReading> _riskAnalysisSensorReadings = new();
    public IReadOnlyCollection<RiskAnalysisSensorReading> RiskAnalysisSensorReadings => _riskAnalysisSensorReadings;

    private readonly List<RiskDataSource> _riskDataSources = new();
    public IReadOnlyCollection<RiskDataSource> RiskDataSources => _riskDataSources;

    private RiskAnalysis() { }

    public RiskAnalysis(Guid cityId, DateTimeOffset analyzedAt)
    {
        if (cityId == Guid.Empty)
            throw new ArgumentException("CityId is required.");

        Id = Guid.NewGuid();
        CityId = cityId;
        AnalyzedAt = analyzedAt;
        CreatedAt = DateTimeOffset.UtcNow;
        RiskLevel = RiskLevel.Low;
    }

    public void UpdateHydrology(decimal? forecastRain24hMm, decimal? accumulatedRain7dMm, decimal? currentOrForecastFlow, decimal? maxFlowNextDays, decimal? terrainElevationMeters, decimal? detectedWaterPercentage)
    {
        if (detectedWaterPercentage is not null && (detectedWaterPercentage < 0 || detectedWaterPercentage > 100))
            throw new ArgumentException("Detected water percentage must be between 0 and 100.");

        ForecastRain24hMm = forecastRain24hMm;
        AccumulatedRain7dMm = accumulatedRain7dMm;
        CurrentOrForecastFlow = currentOrForecastFlow;
        MaxFlowNextDays = maxFlowNextDays;
        TerrainElevationMeters = terrainElevationMeters;
        DetectedWaterPercentage = detectedWaterPercentage;
    }

    public void Classify(int riskScore, RiskLevel riskLevel, string? alertMessage)
    {
        if (riskScore < 0 || riskScore > 100)
            throw new ArgumentException("Risk score must be between 0 and 100.");

        RiskScore = riskScore;
        RiskLevel = riskLevel;
        AlertMessage = string.IsNullOrWhiteSpace(alertMessage) ? null : alertMessage.Trim();
    }
}
