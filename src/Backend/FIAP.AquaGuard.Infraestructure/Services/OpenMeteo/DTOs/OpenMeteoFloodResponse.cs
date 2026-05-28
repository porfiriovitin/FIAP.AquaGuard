using System.Text.Json.Serialization;

namespace FIAP.AquaGuard.Infrastructure.Services.OpenMeteo.DTOs;

public class OpenMeteoFloodResponse
{
    [JsonPropertyName("daily")]
    public DailyFloodResponse? Daily { get; set; }
}

public class DailyFloodResponse
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }

    [JsonPropertyName("river_discharge")]
    public List<double>? RiverDischarge { get; set; }
}
