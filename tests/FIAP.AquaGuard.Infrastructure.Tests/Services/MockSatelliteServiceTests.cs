using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Infrastructure.Services;
using FluentAssertions;

namespace FIAP.AquaGuard.Infrastructure.Tests.Services;

public class MockSatelliteServiceTests
{
    [Fact]
    public async Task GetWaterDetectionResult_ShouldReturnPercentageBetween0And100()
    {
        var openMeteoProvider = new StubOpenMeteoProvider(
            rain: new RainForecast(new Coordinates(-23.55, -46.63), 80, 170, new Hourly([], [])),
            flood: new FloodForecast(new Coordinates(-23.55, -46.63), 300, 650, true, "m3/s", null),
            elevation: new ElevationResult(new Coordinates(-23.55, -46.63), 20));

        var service = new MockSatelliteService(openMeteoProvider);

        var result = await service.GetWaterDetectionResult(new Coordinates(-23.55, -46.63));

        result.WaterPercentage.Should().BeGreaterThanOrEqualTo(0);
        result.WaterPercentage.Should().BeLessThanOrEqualTo(100);
        result.AmountOfImagesFound.Should().BeGreaterThan(0);
        result.DataSource.Should().Be("sentinel-1");
    }

    private sealed class StubOpenMeteoProvider : IOpenMeteoProvider
    {
        private readonly RainForecast _rain;
        private readonly FloodForecast _flood;
        private readonly ElevationResult _elevation;

        public StubOpenMeteoProvider(RainForecast rain, FloodForecast flood, ElevationResult elevation)
        {
            _rain = rain;
            _flood = flood;
            _elevation = elevation;
        }

        public Task<RainForecast> GetRainForecast(Coordinates coordinates, CancellationToken cancellationToken = default) => Task.FromResult(_rain);
        public Task<FloodForecast> GetFloodForecast(Coordinates coordinates, CancellationToken cancellationToken = default) => Task.FromResult(_flood);
        public Task<ElevationResult> GetElevation(Coordinates coordinates, CancellationToken cancellationToken = default) => Task.FromResult(_elevation);
    }
}
