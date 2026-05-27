namespace FIAP.AquaGuard.Domain.Entities;

public class SensorReading
{
    public Guid Id { get; private set; }
    public Guid SensorId { get; private set; }
    public decimal? WaterLevelCm { get; private set; }
    public decimal? FlowRateM3s { get; private set; }
    public decimal? RainfallMm { get; private set; }
    public decimal? BatteryLevel { get; private set; }
    public decimal? SignalStrength { get; private set; }
    public DateTimeOffset MeasuredAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public Sensor Sensor { get; private set; } = null!;

    private readonly List<RiskAnalysisSensorReading> _riskAnalysisSensorReadings = new();
    public IReadOnlyCollection<RiskAnalysisSensorReading> RiskAnalysisSensorReadings => _riskAnalysisSensorReadings;

    private SensorReading() { }

    public SensorReading(Guid sensorId, DateTimeOffset measuredAt, decimal? waterLevelCm = null, decimal? flowRateM3s = null, decimal? rainfallMm = null, decimal? batteryLevel = null, decimal? signalStrength = null)
    {
        if (sensorId == Guid.Empty)
            throw new ArgumentException("SensorId is required.");

        ValidatePercentage(batteryLevel, "Battery level");
        ValidatePercentage(signalStrength, "Signal strength");

        Id = Guid.NewGuid();
        SensorId = sensorId;
        MeasuredAt = measuredAt;
        WaterLevelCm = waterLevelCm;
        FlowRateM3s = flowRateM3s;
        RainfallMm = rainfallMm;
        BatteryLevel = batteryLevel;
        SignalStrength = signalStrength;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateTelemetry(decimal? waterLevelCm, decimal? flowRateM3s, decimal? rainfallMm, decimal? batteryLevel, decimal? signalStrength)
    {
        ValidatePercentage(batteryLevel, "Battery level");
        ValidatePercentage(signalStrength, "Signal strength");

        WaterLevelCm = waterLevelCm;
        FlowRateM3s = flowRateM3s;
        RainfallMm = rainfallMm;
        BatteryLevel = batteryLevel;
        SignalStrength = signalStrength;
    }

    private static void ValidatePercentage(decimal? value, string fieldName)
    {
        if (value is not null && (value < 0 || value > 100))
            throw new ArgumentException($"{fieldName} must be between 0 and 100.");
    }
}
