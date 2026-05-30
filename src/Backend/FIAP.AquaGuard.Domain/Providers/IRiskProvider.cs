using FIAP.AquaGuard.Domain.Models;

namespace FIAP.AquaGuard.Domain.Providers;

public interface IRiskProvider
{
    Task<FloodRiskResult> GetFloodRiskAsync(Coordinates coordinates, CancellationToken cancellationToken = default);

    Task<FloodRiskResultWithSensor> GetFloodRiskWithSensorAsync(Coordinates coordinates, Guid cityId, CancellationToken cancellationToken = default);

}
