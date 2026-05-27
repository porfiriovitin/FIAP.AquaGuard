using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IRiskAnalysisSensorReadingRepository
{
    Task AddAsync(RiskAnalysisSensorReading riskAnalysisSensorReading);
    Task<RiskAnalysisSensorReading?> GetByIdAsync(Guid id);
    Task DeleteAsync(RiskAnalysisSensorReading riskAnalysisSensorReading);
    Task UpdateAsync(RiskAnalysisSensorReading riskAnalysisSensorReading);
}
