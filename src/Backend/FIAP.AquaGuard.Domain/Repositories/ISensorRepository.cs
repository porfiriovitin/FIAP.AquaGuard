using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface ISensorRepository
{
    Task AddAsync(Sensor sensor);
    Task<Sensor?> GetByIdAsync(Guid id);
    Task DeleteAsync(Sensor sensor);
    Task UpdateAsync(Sensor sensor);

    Task<(List<Sensor> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10, Guid cityId = default);
    Task ChangeSensorStatus(Sensor sensor, SensorStatus status);
}
