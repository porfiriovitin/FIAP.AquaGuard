namespace FIAP.AquaGuard.Domain.Entities;

public class SensorReading
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public decimal? WaterLevelCm { get; set; }
    public decimal? FlowRateM3s { get; set; }
    public decimal? RainfallMm { get; set; }
    public decimal? BatteryLevel { get; set; }
    public decimal? SignalStrength { get; set; }
    public DateTimeOffset MeasuredAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Sensor Sensor { get; set; } = null!;
    public ICollection<RiskAnalysisSensorReading> RiskAnalysisSensorReadings { get; set; } = new List<RiskAnalysisSensorReading>();
}
