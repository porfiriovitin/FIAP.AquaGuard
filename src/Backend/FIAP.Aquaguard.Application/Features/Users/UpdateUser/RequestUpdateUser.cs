using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Users.UpdateUser;

public record RequestUpdateUser(Guid UserId, string Name, string Email, Guid RequestedByUserId, UserRole RequestedByRole);
