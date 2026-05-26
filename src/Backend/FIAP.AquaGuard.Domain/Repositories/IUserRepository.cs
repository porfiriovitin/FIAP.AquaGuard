using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(long id);
    Task DeleteAsync(User user);
    Task UpdateAsync(User user);
}
