using FIAP.AquaGuard.Domain.Entities;

namespace FIAP.AquaGuard.Domain.Repositories;

public interface IRiskDataSourceRepository
{
    Task AddAsync(RiskDataSource riskDataSource);
    Task<RiskDataSource?> GetByIdAsync(Guid id);
    Task DeleteAsync(RiskDataSource riskDataSource);
    Task UpdateAsync(RiskDataSource riskDataSource);
}
