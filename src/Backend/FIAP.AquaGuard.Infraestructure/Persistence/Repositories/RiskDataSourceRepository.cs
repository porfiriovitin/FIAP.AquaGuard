using Fiap.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

public class RiskDataSourceRepository : IRiskDataSourceRepository
{
    private readonly AppDbContext _context;

    public RiskDataSourceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RiskDataSource riskDataSource)
    {
        await _context.RiskDataSources.AddAsync(riskDataSource);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RiskDataSource riskDataSource)
    {
        _context.RiskDataSources.Remove(riskDataSource);
        await _context.SaveChangesAsync();
    }

    public async Task<RiskDataSource?> GetByIdAsync(Guid id)
    {
        return await _context.RiskDataSources.FindAsync(id);
    }

    public async Task UpdateAsync(RiskDataSource riskDataSource)
    {
        _context.RiskDataSources.Update(riskDataSource);
        await _context.SaveChangesAsync();
    }
}
