namespace FIAP.AquaGuard.Domain.Models;

public enum RiskLevel
{
    Low,
    Moderate,
    High,
    Critical
}

public sealed record FloodRiskSources(
    string Weather = "open-meteo-forecast",
    string RiverFlow = "open-meteo-flood",
    string Elevation = "open-meteo-elevation",
    string Satellite = "earth-engine"
);

public sealed record FloodRiskResult(
    Coordinates Coordinates,
    double ForecastRain24hMm,
    double AccumulatedRain7dMm,
    double? CurrentOrForecastRiverFlow,
    double? MaxRiverFlowNextDays,
    double? TerrainElevationMeters,
    double WaterDetectedPercentage,
    int RiskScore,
    RiskLevel RiskLevel,
    string AlertMessage,
    FloodRiskSources Sources
);

public sealed record FloodRiskResultWithSensor(
    Coordinates Coordinates,
    double ForecastRain24hMm,
    double AccumulatedRain7dMm,
    double? CurrentOrForecastRiverFlow,
    double? MaxRiverFlowNextDays,
    double? TerrainElevationMeters,
    double WaterDetectedPercentage,
    List<SensorReadingResult> Sensors,
    int RiskScore,
    RiskLevel RiskLevel,
    string AlertMessage,
    FloodRiskSources Sources
    );
