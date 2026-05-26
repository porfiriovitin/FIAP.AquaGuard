using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class RiskDataSource
{
    public Guid Id { get; set; }
    public Guid RiskAnalysisId { get; set; }
    public RiskSourceType SourceType { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string? RawUrl { get; set; }
    public DateTimeOffset RetrievedAt { get; set; }

    public RiskAnalysis RiskAnalysis { get; set; } = null!;
}
