using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class RiskDataSource
{
    public Guid Id { get; private set; }
    public Guid RiskAnalysisId { get; private set; }
    public RiskSourceType SourceType { get; private set; }
    public string ProviderName { get; private set; } = string.Empty;
    public string? RawUrl { get; private set; }
    public DateTimeOffset RetrievedAt { get; private set; }

    public RiskAnalysis RiskAnalysis { get; private set; } = null!;

    private RiskDataSource() { }

    public RiskDataSource(Guid riskAnalysisId, RiskSourceType sourceType, string providerName, DateTimeOffset retrievedAt, string? rawUrl = null)
    {
        if (riskAnalysisId == Guid.Empty)
            throw new ArgumentException("RiskAnalysisId is required.");

        if (string.IsNullOrWhiteSpace(providerName))
            throw new ArgumentException("Provider name is required.");

        Id = Guid.NewGuid();
        RiskAnalysisId = riskAnalysisId;
        SourceType = sourceType;
        ProviderName = providerName.Trim();
        RawUrl = string.IsNullOrWhiteSpace(rawUrl) ? null : rawUrl.Trim();
        RetrievedAt = retrievedAt;
    }
}
