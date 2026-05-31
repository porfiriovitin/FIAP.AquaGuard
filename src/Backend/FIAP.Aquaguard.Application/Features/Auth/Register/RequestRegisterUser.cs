using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public record RequestRegisterUser(
    string Name,
    string Email,
    string Password,
    Guid? CityId,
    UserRole? Role 
 );




