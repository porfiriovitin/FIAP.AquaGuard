using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Infrastructure.Services;
using FluentAssertions;

namespace FIAP.AquaGuard.Infrastructure.Tests.Services;

public class MockSensorServiceTests
{
    [Fact]
    public async Task GetSensorResult_ShouldThrowInvalidOperationException_WhenNoActiveSensors()
    {
        var sensorRepository = new StubSensorRepository(new List<Sensor>());
        var openMeteo = new StubOpenMeteoProvider();
        var service = new MockSensorService(sensorRepository, openMeteo);

        Func<Task> act = () => service.GetSensorResult(Guid.NewGuid());

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetSensorResult_ShouldReturnOneReadingPerActiveSensor()
    {
        var cityId = Guid.NewGuid();
        var activeSensor = new Sensor(cityId, "Centro", -23.55m, -46.63m, SensorType.WaterLevel, DateTimeOffset.UtcNow);
        var inactiveSensor = new Sensor(cityId, "Bairro", -23.56m, -46.64m, SensorType.WaterLevel, DateTimeOffset.UtcNow);
        inactiveSensor.ChangeStatus(SensorStatus.Inactive);

        var sensorRepository = new StubSensorRepository(new List<Sensor> { activeSensor, inactiveSensor });
        var openMeteo = new StubOpenMeteoProvider();
        var service = new MockSensorService(sensorRepository, openMeteo);

        var result = await service.GetSensorResult(cityId);

        result.Should().HaveCount(1);
        result[0].SensorId.Should().Be(activeSensor.Id);
        result[0].WaterLevelCm.Should().BeGreaterThan(0);
        result[0].BatteryLevel.Should().BeInRange(10, 100);
        result[0].SignalStrength.Should().BeInRange(5, 100);
    }

    private sealed class StubOpenMeteoProvider : IOpenMeteoProvider
    {
        public Task<RainForecast> GetRainForecast(Coordinates coordinates, CancellationToken cancellationToken = default)
            => Task.FromResult(new RainForecast(coordinates, 40, 120, new Hourly([], [])));

        public Task<FloodForecast> GetFloodForecast(Coordinates coordinates, CancellationToken cancellationToken = default)
            => Task.FromResult(new FloodForecast(coordinates, 220, 450, true, "m3/s", null));

        public Task<ElevationResult> GetElevation(Coordinates coordinates, CancellationToken cancellationToken = default)
            => Task.FromResult(new ElevationResult(coordinates, 35));
    }

    private sealed class StubSensorRepository : ISensorRepository
    {
        private readonly List<Sensor> _items;

        public StubSensorRepository(List<Sensor> items)
        {
            _items = items;
        }

        public Task<(List<Sensor> Items, int Total)> GetAllAsync(int page = 1, int pageSize = 10, Guid cityId = default)
            => Task.FromResult((_items, _items.Count));

        public Task AddAsync(Sensor sensor) => Task.CompletedTask;
        public Task<Sensor?> GetByIdAsync(Guid id) => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));
        public Task DeleteAsync(Sensor sensor) => Task.CompletedTask;
        public Task UpdateAsync(Sensor sensor) => Task.CompletedTask;
        public Task ChangeSensorStatus(Sensor sensor, SensorStatus status) => Task.CompletedTask;
    }
}
