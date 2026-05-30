using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using System.Globalization;

namespace FIAP.AquaGuard.Infrastructure.Services;

/// [FIAP allowed us to mock complex integrations due the few time we had to implement the project, so this service simulates a satellite provider by using data from Open-Meteo to create a water detection result. It's not based on real satellite data, but it combines weather forecasts and elevation data to generate a plausible water detection result for demonstration purposes.] 
public class MockSatelliteService : ISatelliteProvider
{
    private readonly IOpenMeteoProvider _openMeteoProvider;

    public MockSatelliteService(IOpenMeteoProvider openMeteoProvider)
    {
        _openMeteoProvider = openMeteoProvider;
    }

    public async Task<WaterDetectionResult> GetWaterDetectionResult(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        /// :: Gets data from Open-Meteo and combines it to create a mock water detection result.
        var rainTask = _openMeteoProvider.GetRainForecast(coordinates, cancellationToken);
        var floodTask = _openMeteoProvider.GetFloodForecast(coordinates, cancellationToken);
        var elevationTask = _openMeteoProvider.GetElevation(coordinates, cancellationToken);

        /// :: Waits for all data to be fetched in parallel.
        await Task.WhenAll(rainTask, floodTask, elevationTask);

        /// :: Base.
        var rain = await rainTask;
        var flood = await floodTask;
        var elevation = await elevationTask;

        /// :: Combines the data to create a mock water detection result.
        var rain24hScore = Normalize(rain.Rain24hMm, max: 120);
        var rain7dScore = Normalize(rain.Rain7dMm, max: 300);
        var dischargeScore = Normalize(flood.MaximumDischargeNextDays ?? 0, max: 800);
        var lowElevationScore = elevation.ElevationMeters is null? 0.25 : 1 - Normalize(elevation.ElevationMeters.Value, max: 1000);

        /// :: If there's a high discharge forecast, it increases the flood potential by 10%.
        var highDischargeBonus = flood.IsHighDischargeForecasted ? 0.1 : 0;

        /// :: Calculates the flood potential as a weighted average of the different factors.
        var floodPotential =(0.45 * rain24hScore) +(0.25 * rain7dScore) +(0.20 * dischargeScore) + (0.10 * lowElevationScore) + highDischargeBonus;

        /// :: Converts the flood potential to a water percentage and rounds it to 2 decimal places.
        var waterPercentage = Math.Round(Clamp01(floodPotential) * 100, 2);

        /// :: Uses the coordinates hash to determine a base amount of images found, and adds extra images based on the water percentage.
        var hash = GetCoordinateHash(coordinates);
        var baseImages = 4 + (hash % 12); // [4..15]
        var riskExtraImages = (long)Math.Round(waterPercentage / 10); // [0..10]
        var amountOfImagesFound = baseImages + riskExtraImages;

        return new WaterDetectionResult(
            Coordinates: coordinates,
            WaterPercentage: waterPercentage,
            ImageData: null,
            AmountOfImagesFound: amountOfImagesFound,
            DataSource: "open-meteo+mock-satellite"
        );
    }

    /// <summary>
    /// Normalizes a value to a range of 0 to 1 based on a specified maximum. If the maximum is zero or negative, it returns 0 to avoid division by zero.
    /// </summary>
    private static double Normalize(double value, double max)
    {
        if (max <= 0)
            return 0;

        return Clamp01(value / max);
    }

    /// <summary>
    /// Clamps a value to be between 0 and 1. If the value is less than 0, it returns 0. If the value is greater than 1, it returns 1. Otherwise, it returns the original value.
    /// </summary>
    private static double Clamp01(double value)
    {
        if (value < 0)
            return 0;

        if (value > 1)
            return 1;

        return value;
    }

    /// <summary>
    /// Gets a hash code for the coordinates by converting the latitude and longitude to strings with 4 decimal places, concatenating them, and then getting the hash code of the resulting string. The absolute value of the hash code is returned to ensure it's non-negative.
    /// </summary>
    private static int GetCoordinateHash(Coordinates coordinates)
    {
        var latitude = coordinates.Latitude.ToString("F4", CultureInfo.InvariantCulture);
        var longitude = coordinates.Longitude.ToString("F4", CultureInfo.InvariantCulture);
        var key = $"{latitude}:{longitude}";

        return Math.Abs(key.GetHashCode());
    }
}
