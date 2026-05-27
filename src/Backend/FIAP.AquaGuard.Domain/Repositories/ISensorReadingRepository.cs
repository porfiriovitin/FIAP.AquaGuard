using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface ISensorReadingRepository
{
    Task AddAsync(SensorReading sensorReading);
    Task<SensorReading?> GetByIdAsync(Guid id);
    Task DeleteAsync(SensorReading sensorReading);
    Task UpdateAsync(SensorReading sensorReading);
}
