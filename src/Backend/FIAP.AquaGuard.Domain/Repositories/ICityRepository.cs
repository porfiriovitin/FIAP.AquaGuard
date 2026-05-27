using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface ICityRepository
{
    Task AddAsync(City city);
    Task<City?> GetByIdAsync(Guid id);
    Task<City?> GetByZipCode(string zipCode);
    Task<City?> GetByNameAsync(string name);
    Task<(List<City> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10);
    Task DeleteAsync(City city);
    Task UpdateAsync(City city);
}
