using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(City city)
    {
        await _context.Cities.AddAsync(city);
    }

    public async Task DeleteAsync(City city)
    {
        _context.Cities.Remove(city);
    }

    public async Task<(List<City> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var total = await _context.Cities.CountAsync();

        var items = await _context.Cities.AsNoTracking().OrderBy(c => c.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, total);
    }

    public async Task<City?> GetByCoordinates(Coordinates coordinates)
    {
        var latitude = Convert.ToDecimal(coordinates.Latitude).ToString(CultureInfo.InvariantCulture);
        var longitude = Convert.ToDecimal(coordinates.Longitude).ToString(CultureInfo.InvariantCulture);

        return await _context.Cities
            .FirstOrDefaultAsync(y =>
                EF.Functions.Like(y.Latitude.ToString(), $"{latitude}%") &&
                EF.Functions.Like(y.Longitude.ToString(), $"{longitude}%"));
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<City?> GetByNameAsync(string name)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<City?> GetByZipCode(string zipCode)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Zipcode == zipCode);
    }

    public async Task UpdateAsync(City city)
    {
        _context.Cities.Update(city);
    }
}
