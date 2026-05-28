using FIAP.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly AppDbContext _context;

    public SensorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Sensor sensor)
    {
        await _context.Sensors.AddAsync(sensor);
    }

    public async Task DeleteAsync(Sensor sensor)
    {
        _context.Sensors.Remove(sensor);
    }

    public async Task<(List<Sensor> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10, Guid cityId = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var total = await _context.Sensors.Where(s => s.CityId == cityId).CountAsync();

        var items = await _context.Sensors
            .Where(y => y.CityId == cityId)
            .AsNoTracking()
            .Include(s => s.City.Name)
            .OrderBy(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Sensor?> GetByIdAsync(Guid id)
    {
        return await _context.Sensors.FindAsync(id);
    }

    public async Task UpdateAsync(Sensor sensor)
    {
        _context.Sensors.Update(sensor);
    }
}
