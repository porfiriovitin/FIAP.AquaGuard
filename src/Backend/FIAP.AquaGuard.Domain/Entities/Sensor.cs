using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public SensorType SensorType { get; set; }
    public SensorStatus Status { get; set; }
    public DateTimeOffset InstalledAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public City City { get; set; } = null!;
    public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
}
