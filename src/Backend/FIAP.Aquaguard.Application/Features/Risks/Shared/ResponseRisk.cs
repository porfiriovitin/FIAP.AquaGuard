using FIAP.AquaGuard.Domain.Models;

namespace FIAP.Aquaguard.Application.Features.Risks.Shared;

public record ResponseRiskSources(
    string Weather,
    string RiverFlow,
    string Elevation,
    string Satellite);

public record ResponseRiskSensor(
    Guid SensorId,
    string PlaceName,
    double WaterLevelCm,
    double FlowRateM3s,
    double RainfallMm,
    double BatteryLevel,
    double SignalStrength,
    DateTimeOffset MeasuredAt);

public record ResponseRiskSimple(
    double Latitude,
    double Longitude,
    double ForecastRain24hMm,
    double AccumulatedRain7dMm,
    double? CurrentOrForecastRiverFlow,
    double? MaxRiverFlowNextDays,
    double? TerrainElevationMeters,
    double WaterDetectedPercentage,
    int RiskScore,
    RiskLevel RiskLevel,
    string AlertMessage,
    ResponseRiskSources Sources);

public record ResponseRiskComplete(
    double Latitude,
    double Longitude,
    double ForecastRain24hMm,
    double AccumulatedRain7dMm,
    double? CurrentOrForecastRiverFlow,
    double? MaxRiverFlowNextDays,
    double? TerrainElevationMeters,
    double WaterDetectedPercentage,
    List<ResponseRiskSensor> Sensors,
    int RiskScore,
    RiskLevel RiskLevel,
    string AlertMessage,
    ResponseRiskSources Sources);

public record ResponseRiskByCoordinates(
    bool HasCityMatch,
    Guid? CityId,
    ResponseRiskSimple? SimpleRisk,
    ResponseRiskComplete? CompleteRisk);
