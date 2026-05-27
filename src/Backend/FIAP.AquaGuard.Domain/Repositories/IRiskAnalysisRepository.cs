using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IRiskAnalysisRepository
{
    Task AddAsync(RiskAnalysis riskAnalysis);
    Task<RiskAnalysis?> GetByIdAsync(Guid id);
    Task DeleteAsync(RiskAnalysis riskAnalysis);
    Task UpdateAsync(RiskAnalysis riskAnalysis);
    Task<(List<RiskAnalysis> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10, Guid cityId = default);
}
