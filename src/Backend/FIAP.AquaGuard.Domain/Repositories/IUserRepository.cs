using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    void Delete(User user);
    void Update(User user);
}
