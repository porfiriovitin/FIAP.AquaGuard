using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Providers;

namespace FIAP.AquaGuard.Infrastructure.Services
{
    public class BCryptService : IPasswordHasherProvider
    {
        /// <summary>
        /// Creates a password hash using the BCrypt algorithm.
        /// </summary>
        public string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Compares a plaintext password with a hashed password to verify if they match, using the BCrypt algorithm.
        /// </summary>
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
