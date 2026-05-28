using System.Text.Json.Serialization;

namespace FIAP.AquaGuard.Infrastructure.Services.OpenMeteo.DTOs;

public class OpenMeteoElevationResponse
{
    [JsonPropertyName("elevation")]
    public List<double>? Elevation { get; set; }
}
