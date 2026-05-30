using FIAP.AquaGuard.Domain.Models;

namespace FIAP.AquaGuard.Domain.Providers;

public interface IOpenMeteoProvider
{
    Task<RainForecast> GetRainForecast(Coordinates coordinates, CancellationToken cancellationToken = default);

    Task<FloodForecast> GetFloodForecast( Coordinates coordinates, CancellationToken cancellationToken = default);

    Task<ElevationResult> GetElevation(Coordinates coordinates, CancellationToken cancellationToken = default);
}
