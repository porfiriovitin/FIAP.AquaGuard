using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

public enum UserRole
{
    [PgName("Admin")]
    Admin,

    [PgName("Manager")]
    Manager,

    [PgName("Employee")]
    Employee,

    [PgName("Resident")]
    Resident
}
