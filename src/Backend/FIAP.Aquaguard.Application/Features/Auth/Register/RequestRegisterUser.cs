namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public record RequestRegisterUser (string Name, string Email, string Password, string? Role, string? Zipcode);