namespace FIAP.AquaGuard.Domain.Entities;

public class RiskAnalysisSensorReading
{
    public Guid Id { get; set; }
    public Guid RiskAnalysisId { get; set; }
    public Guid SensorReadingId { get; set; }

    public RiskAnalysis RiskAnalysis { get; set; } = null!;
    public SensorReading SensorReading { get; set; } = null!;
}
