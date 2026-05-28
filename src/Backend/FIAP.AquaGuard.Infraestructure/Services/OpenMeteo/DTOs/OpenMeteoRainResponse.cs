using System.Text.Json.Serialization;

namespace FIAP.AquaGuard.Infrastructure.Services.OpenMeteo.DTOs
{
    public class OpenMeteoRainResponse
    {
        [JsonPropertyName("hourly")]
        public HourlyRainResponse? Hourly { get; set; }
    }

    public class HourlyRainResponse
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }

        [JsonPropertyName("precipitation")]
        public List<double>? Precipitation { get; set; }
    }
}
