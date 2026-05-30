using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;

namespace FIAP.AquaGuard.Infrastructure.Services;

/// :: [FIAP allowed us to mock IoT integrations due the few time we had to implement the project, so this service simulates sensor telemetry by using Open-Meteo data to generate plausible river readings for demonstration purposes.]
public class MockSensorService : ISensorProvider
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IOpenMeteoProvider _openMeteoProvider;

    public MockSensorService(ISensorRepository sensorRepository, IOpenMeteoProvider openMeteoProvider)
    {
        _sensorRepository = sensorRepository;
        _openMeteoProvider = openMeteoProvider;
    }

    public async Task<List<SensorReadingResult>> GetSensorResult(Guid cityId)
    {
        /// :: Get active sensors for the city. In a real implementation, this would fetch live data from the sensors, but here we simulate it using weather forecasts.
        var (items, _) = await _sensorRepository.GetAllAsync(cityId: cityId);
        List<Sensor> sensors = items.Where(x => x.Status == SensorStatus.Active).ToList();

        if (sensors.Count == 0)
            throw new InvalidOperationException("No active sensors found for this city.");

        /// :: Create a list to hold the simulated sensor readings. We will generate plausible water level, flow rate, and rainfall data based on the Open-Meteo forecasts for each sensor's location.
        var readings = new List<SensorReadingResult>(sensors.Count);
        
        /// :: Iterate over each sensor.
        foreach (var sensor in sensors)
        {
            /// :: Gets the coordinates from each sensor.
            var coordinates = new Coordinates((double)sensor.Latitude, (double)sensor.Longitude);

            /// :: Fetch the rain and flood forecasts concurrently to optimize response time. The rain forecast will give us the expected rainfall in the last 24 hours and 7 days, while the flood forecast will provide information on current and forecasted river discharge.
            var rainTask = _openMeteoProvider.GetRainForecast(coordinates);
            var floodTask = _openMeteoProvider.GetFloodForecast(coordinates);

            await Task.WhenAll(rainTask, floodTask);

            var rain = await rainTask;
            var flood = await floodTask;

            /// :: Calculates the rainfall in millimeters and the river discharge in cubic meters per second, ensuring that we don't have negative values. These will be used as inputs to our sensor reading simulation.
            var rainfallMm = Math.Round(Math.Max(0, rain.Rain24hMm), 2);
            var dischargeM3s = Math.Round(Math.Max(0, flood.CurrentOrForecastedDischarge ?? 0), 2);

            /// :: Normalizes the rain and discharge values to a 0-1 scale based on reasonable maximums (120mm for 24h rain, 300mm for 7d rain, and 900 m3/s for discharge). These normalized scores will be used to calculate a "flood pressure" that influences the simulated water level.
            var rain24hScore = Normalize(rain.Rain24hMm, 120);
            var rain7dScore = Normalize(rain.Rain7dMm, 300);
            var dischargeScore = Normalize(flood.MaximumDischargeNextDays ?? dischargeM3s, 900);
            var highDischargeBonus = flood.IsHighDischargeForecasted ? 0.12 : 0;

            /// :: Combines the normalized scores into a single "flood pressure" metric using weighted contributions. The 24h rain has the highest weight, followed by discharge and then 7d rain. If a high discharge is forecasted, it adds a bonus to the flood pressure.
            var floodPressure = Clamp01((0.45 * rain24hScore) + (0.20 * rain7dScore) + (0.35 * dischargeScore) + highDischargeBonus);

            /// :: Simulates the water level in centimeters based on the flood pressure. The formula starts with a base level of 60 cm and adds up to 380 cm depending on the flood pressure. This creates a plausible range of water levels that would correspond to the given weather conditions.
            var waterLevelCm = Math.Round(60 + (floodPressure * 380), 2);

            /// :: Adds deterministic location variance to avoid identical telemetry in all cities.
            var variance = GetLocationVariance(coordinates);
            waterLevelCm = Math.Round(Math.Max(15, waterLevelCm + (variance * 18)), 2);

            /// :: Simulated battery/signal decay mildly linked to weather stress.
            var batteryLevel = Math.Round(Math.Clamp(95 - (floodPressure * 22) - Math.Abs(variance * 8), 10, 100), 2);
            var signalStrength = Math.Round(Math.Clamp(90 - (rain24hScore * 18) - Math.Abs(variance * 10), 5, 100), 2);

            /// :: Creates a new SensorReadingResult with the simulated values and adds it to the list of readings. Each reading includes the sensor ID, water level, flow rate, rainfall, battery level, signal strength, and the timestamp of when the measurement was taken.
            readings.Add(new SensorReadingResult(
                SensorId: sensor.Id,
                WaterLevelCm: waterLevelCm,
                FlowRateM3s: dischargeM3s,
                RainfallMm: rainfallMm,
                BatteryLevel: batteryLevel,
                SignalStrength: signalStrength,
                MeasuredAt: DateTimeOffset.UtcNow
            ));
        }

        return readings;
    }

    private static double Normalize(double value, double max)
    {
        if (max <= 0)
            return 0;

        return Clamp01(value / max);
    }

    private static double Clamp01(double value)
    {
        if (value < 0)
            return 0;

        if (value > 1)
            return 1;

        return value;
    }

    private static double GetLocationVariance(Coordinates coordinates)
    {
        var seed = HashCode.Combine(
            Math.Round(coordinates.Latitude, 3),
            Math.Round(coordinates.Longitude, 3));

        var normalized = (Math.Abs(seed) % 2000) / 1000.0; // [0..2]
        return normalized - 1.0; // [-1..1]
    }
}
