using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    void Delete(User user);
    void Update(User user);
    Task<(List<User> Users, int Total)> ListUsersPerCityAsync(int page = 1, int pageSize = 10, Guid cityId = default);
    Task<(List<User> Users, int Total)> ListUsersAsync(int page = 1, int pageSize = 10);
}
