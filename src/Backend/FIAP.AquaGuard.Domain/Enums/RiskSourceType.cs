using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

public enum RiskSourceType
{
    [PgName("Weather")]
    Weather,
    [PgName("Flow")]
    Flow,
    [PgName("Elevation")]
    Elevation,
    [PgName("Satellite")]
    Satellite,
    [PgName("Sensor")]
    Sensor
}
