namespace FIAP.Aquaguard.Application.Features.Auth.Login;

public record ResponseLogin(string Name, string Email, string Role, string Token);

public record ResponseLoginDTO(string Name, string Email, string Role);
