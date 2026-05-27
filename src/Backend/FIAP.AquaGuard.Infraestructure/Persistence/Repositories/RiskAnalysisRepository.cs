using Fiap.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

public class RiskAnalysisRepository : IRiskAnalysisRepository
{
    private readonly AppDbContext _context;

    public RiskAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RiskAnalysis riskAnalysis)
    {
        await _context.RiskAnalyses.AddAsync(riskAnalysis);
    }

    public async Task DeleteAsync(RiskAnalysis riskAnalysis)
    {
        _context.RiskAnalyses.Remove(riskAnalysis);
    }

    public async Task<(List<RiskAnalysis> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10, Guid cityId = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var total = await _context.RiskAnalyses.Where(y => y.CityId == cityId).CountAsync();

        var items = await _context.RiskAnalyses
            .AsNoTracking()
            .Where(r => r.CityId == cityId)
            .OrderByDescending(r => r.AnalyzedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);

    }

    public async Task<RiskAnalysis?> GetByIdAsync(Guid id)
    {
        return await _context.RiskAnalyses.FindAsync(id);
    }

    public async Task UpdateAsync(RiskAnalysis riskAnalysis)
    {
        _context.RiskAnalyses.Update(riskAnalysis);
    }
}
