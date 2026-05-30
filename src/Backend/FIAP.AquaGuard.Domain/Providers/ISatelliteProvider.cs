using FIAP.AquaGuard.Domain.Models;

namespace FIAP.AquaGuard.Domain.Providers;

public interface ISatelliteProvider
{
    Task<WaterDetectionResult> GetWaterDetectionResult(Coordinates coordinates, CancellationToken cancellationToken = default);

}
