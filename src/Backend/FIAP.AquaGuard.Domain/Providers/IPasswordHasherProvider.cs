namespace FIAP.AquaGuard.Domain.Providers;

public interface IPasswordHasherProvider
{
    string CreatePasswordHash(string password);
    bool VerifyPassword(string password, string hashPassword);
}
