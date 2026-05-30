using FIAP.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Repositories;

public class RiskAnalysisSensorReadingRepository : IRiskAnalysisSensorReadingRepository
{
    private readonly AppDbContext _context;

    public RiskAnalysisSensorReadingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RiskAnalysisSensorReading riskAnalysisSensorReading)
    {
        await _context.RiskAnalysisSensorReadings.AddAsync(riskAnalysisSensorReading);
    }

    public async Task DeleteAsync(RiskAnalysisSensorReading riskAnalysisSensorReading)
    {
        _context.RiskAnalysisSensorReadings.Remove(riskAnalysisSensorReading);
    }

    public async Task<RiskAnalysisSensorReading?> GetByIdAsync(Guid id)
    {
        return await _context.RiskAnalysisSensorReadings.FindAsync(id);
    }

    public async Task UpdateAsync(RiskAnalysisSensorReading riskAnalysisSensorReading)
    {
        _context.RiskAnalysisSensorReadings.Update(riskAnalysisSensorReading);
    }
}
