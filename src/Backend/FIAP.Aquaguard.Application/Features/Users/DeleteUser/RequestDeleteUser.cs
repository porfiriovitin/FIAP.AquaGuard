using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Users.DeleteUser;

public record RequestDeleteUser(Guid UserId, Guid RequestedByUserId, UserRole RequestedByRole);
