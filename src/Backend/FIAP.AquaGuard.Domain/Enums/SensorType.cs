using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

public enum SensorType
{
    [PgName("WaterLevel")]
    WaterLevel,

    [PgName("FlowRate")]
    FlowRate,

    [PgName("RainGauge")]
    RainGauge
}

/// :: 0 - Water Level
/// :: 1 - FlowRate
/// :: 2 - RainGauge
