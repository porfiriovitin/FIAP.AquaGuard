using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

public enum RiskLevel
{
    [PgName("Low")]
    Low,
    [PgName("Moderate")]
    Moderate,
    [PgName("High")]
    High,
    [PgName("Critical")]
    Critical
}
