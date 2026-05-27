using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Services;

public interface ITokenProvider
{
    string GenerateToken(User user);

}
