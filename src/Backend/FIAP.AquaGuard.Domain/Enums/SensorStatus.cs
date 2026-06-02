using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

public enum SensorStatus
{
    [PgName("Active")]
    Active,

    [PgName("Inactive")]
    Inactive,

    [PgName("Maintenance")]
    Maintenance
}
