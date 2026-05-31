using NpgsqlTypes;

namespace FIAP.AquaGuard.Domain.Enums;

/// ::	0 para Admin
/// ::	1 para Manager
/// ::	2 para Employee
/// ::	3 para Resident
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
