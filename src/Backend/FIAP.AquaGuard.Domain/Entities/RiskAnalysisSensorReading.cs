namespace FIAP.AquaGuard.Domain.Entities;

public class RiskAnalysisSensorReading
{
    public Guid Id { get; private set; }
    public Guid RiskAnalysisId { get; private set; }
    public Guid SensorReadingId { get; private set; }

    public RiskAnalysis RiskAnalysis { get; private set; } = null!;
    public SensorReading SensorReading { get; private set; } = null!;

    private RiskAnalysisSensorReading() { }

    public RiskAnalysisSensorReading(Guid riskAnalysisId, Guid sensorReadingId)
    {
        if (riskAnalysisId == Guid.Empty)
            throw new ArgumentException("RiskAnalysisId is required.");

        if (sensorReadingId == Guid.Empty)
            throw new ArgumentException("SensorReadingId is required.");

        Id = Guid.NewGuid();
        RiskAnalysisId = riskAnalysisId;
        SensorReadingId = sensorReadingId;
    }
}
