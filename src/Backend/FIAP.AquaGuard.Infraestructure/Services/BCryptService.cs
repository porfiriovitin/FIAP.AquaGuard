using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Providers;

namespace FIAP.AquaGuard.Infrastructure.Services
{
    public class BCryptService : IPasswordHasherProvider
    {
        public string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
