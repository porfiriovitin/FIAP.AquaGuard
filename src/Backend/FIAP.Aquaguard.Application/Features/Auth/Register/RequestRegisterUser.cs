using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Models;

namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public record RequestRegisterUser(
    string Name,
    string Email,
    string Password,
    string? Role,
    string? Zipcode,
    Coordinates? Coordinates,
    UserRole? role 
 );
