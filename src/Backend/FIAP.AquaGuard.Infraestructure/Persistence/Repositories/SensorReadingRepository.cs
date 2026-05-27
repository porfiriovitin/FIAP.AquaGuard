using Fiap.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly AppDbContext _context;

    public SensorReadingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SensorReading sensorReading)
    {
        await _context.SensorReadings.AddAsync(sensorReading);
    }

    public async Task DeleteAsync(SensorReading sensorReading)
    {
        _context.SensorReadings.Remove(sensorReading);
    }

    public async Task<SensorReading?> GetByIdAsync(Guid id)
    {
        return await _context.SensorReadings.FindAsync(id);
    }

    public async Task UpdateAsync(SensorReading sensorReading)
    {
        _context.SensorReadings.Update(sensorReading);
    }
}
