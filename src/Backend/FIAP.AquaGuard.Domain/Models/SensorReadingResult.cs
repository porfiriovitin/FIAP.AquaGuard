namespace FIAP.AquaGuard.Domain.Models;

public record SensorReadingResult(
    Guid SensorId,
    string PlaceName,
    double WaterLevelCm,
    double FlowRateM3s,
    double RainfallMm,
    double BatteryLevel,
    double SignalStrength,
    DateTimeOffset MeasuredAt
);
