using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FIAP.AquaGuard.Infrastructure.Services.OpenMeteo.DTOs;
using System.Globalization;
using System.Text.Json;

namespace FIAP.AquaGuard.Infrastructure.Services.OpenMeteo;

public class OpenMeteoService : IOpenMeteoProvider
{
    /// :: Base
    private const string ForecastAndElevationUrl = "https://api.open-meteo.com/v1";
    private const string FloodForecastUrl = "https://flood-api.open-meteo.com/v1";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _httpClient;

    public OpenMeteoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets rain forecast data for the specified coordinates, including total precipitation for the next 24 hours and 7 days, as well as hourly precipitation values.
    /// </summary>
    public async Task<RainForecast> GetRainForecast(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        /// :: Guardrails to avoid culture-specific formatting issues with decimal points in latitude and longitude.
        var latitude = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
        var longitude = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

        /// :: Builds the API Url with the specified latitude and longitude, requesting hourly precipitation data.
        var url = $"{ForecastAndElevationUrl}/forecast?latitude={latitude}&longitude={longitude}&hourly=precipitation";

        /// :: Makes an asynchronous GET request to the OpenMeteo API and checks for a successful response. If the response is not successful, an exception is thrown.
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new OpenMeteoException("Failed to fetch rain forecast.");

        /// :: Reads the response and deserializes the content into an structured Object.
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<OpenMeteoRainResponse>(json, JsonOptions);

        /// :: Guardrails to handle potential null values in the API response, ensuring that the application does not crash due to unexpected null references.
        var precipitation = data?.Hourly?.Precipitation ?? [];
        var time = data?.Hourly?.Time ?? [];

        /// :: Returns a new RainForecast object populated with the total precipitation for the next 24 hours and 7 days, as well as the hourly precipitation values. 
        return new RainForecast(
            Coordinates: coordinates,
            Rain24hMm: Math.Round(precipitation.Take(24).Sum(), 2),
            Rain7dMm: Math.Round(precipitation.Sum(), 2),
            Hourly: new Hourly(
                Time: time,
                Precipitation: precipitation
            )
        );
    }

    /// <summary>
    /// Gets flood forecast data for the specified coordinates, including current or forecasted river discharge, maximum discharge in the next days, and whether high discharge is forecasted. It also includes daily river discharge values for the next 7 days.
    /// </summary>
    public async Task<FloodForecast> GetFloodForecast(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        /// :: Guardrails to avoid culture-specific formatting issues with decimal points in latitude and longitude.
        var latitude = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
        var longitude = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

        /// :: Builds the API Url with the specified latitude and longitude, requesting hourly precipitation data.
        var url = $"{FloodForecastUrl}/flood?latitude={latitude}&longitude={longitude}&daily=river_discharge&forecast_days=7";

        /// :: Makes an asynchronous GET request to the OpenMeteo API and checks for a successful response. If the response is not successful, an exception is thrown.
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new OpenMeteoException("Failed to fetch flood forecast.");

        /// :: Reads the response and deserializes the content into an structured Object.
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<OpenMeteoFloodResponse>(json, JsonOptions);

        /// :: Guardrails to handle potential null values in the API response, ensuring that the application does not crash due to unexpected null references.
        var riverDischarge = data?.Daily?.RiverDischarge ?? [];
        var time = data?.Daily?.Time ?? [];

        /// :: Filters out any NaN values from the river discharge data to ensure that calculations for current or forecasted discharge and maximum discharge are based on valid numeric values.
        var validValues = riverDischarge.Where(value => !double.IsNaN(value)).ToList();
        double? currentOrNextValue = validValues.FirstOrDefault();

        if (validValues.Count == 0)
            currentOrNextValue = null;

        /// :: Calculates the maximum discharge value from the valid river discharge data. If there are no valid values, max is set to null.
        double? max = validValues.Count > 0? validValues.Max(): null;

        /// :: Defines a threshold for high discharge, which can be adjusted based on specific requirements or historical data for the region. 
        /// :: In this example, a threshold of 500 m3/s is used to determine if high discharge is forecasted.
        const double highThreshold = 500;

        return new FloodForecast(
            Coordinates: coordinates,
            CurrentOrForecastedDischarge: currentOrNextValue,
            MaximumDischargeNextDays: max,
            IsHighDischargeForecasted: max is not null && max >= highThreshold,
            Unit: "m3/s",
            Daily: new DailyDischarge(
                Time: time,
                RiverDischarge: riverDischarge
            )
        );
    }

    /// <summary>
    ///  Gets the elevation data for the specified coordinates, returning the elevation in meters. If the elevation data is not available, it returns null.
    /// </summary>
    public async Task<ElevationResult> GetElevation(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        /// :: Guardrails to avoid culture-specific formatting issues with decimal points in latitude and longitude.
        var latitude = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
        var longitude = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

        /// :: Builds the API Url with the specified latitude and longitude, requesting elevation data.
        var url = $"{ForecastAndElevationUrl}/elevation?latitude={latitude}&longitude={longitude}";

        /// :: Makes an asynchronous GET request to the OpenMeteo API and checks for a successful response. If the response is not successful, an exception is thrown.
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new OpenMeteoException("Failed to fetch elevation data.");

        /// :: Reads the response and deserializes the content into an structured Object.
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<OpenMeteoElevationResponse>(json, JsonOptions);

        /// :: Guardrails to handle potential null values in the API response, ensuring that the application does not crash due to unexpected null references. If elevation data is not available, it returns null.
        double? elevation = data?.Elevation?.FirstOrDefault();

        return new ElevationResult(
            Coordinates: coordinates,
            ElevationMeters: elevation
        );
    }
}
