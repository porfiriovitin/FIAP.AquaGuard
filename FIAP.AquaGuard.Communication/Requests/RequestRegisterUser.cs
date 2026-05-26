namespace FIAP.AquaGuard.Communication.Requests;

public record RequestRegisterUser (string Name, string Email, string Password, string Role);