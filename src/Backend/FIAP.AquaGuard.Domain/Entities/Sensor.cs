using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class Sensor
{
    public Guid Id { get; private set; }
    public Guid CityId { get; private set; }
    public string PlaceName { get; private set; } = string.Empty;
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public SensorType SensorType { get; private set; }
    public SensorStatus Status { get; private set; }
    public DateTimeOffset InstalledAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public City City { get; private set; } = null!;

    private readonly List<SensorReading> _sensorReadings = new();
    public IReadOnlyCollection<SensorReading> SensorReadings => _sensorReadings;

    private Sensor() { }

    public Sensor(Guid cityId, string placeName, decimal latitude, decimal longitude, SensorType sensorType, DateTimeOffset installedAt)
    {
        if (cityId == Guid.Empty)
            throw new ArgumentException("CityId is required.");

        if (string.IsNullOrWhiteSpace(placeName))
            throw new ArgumentException("Place name is required.");

        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude.");

        Id = Guid.NewGuid();
        CityId = cityId;
        PlaceName = placeName.Trim();
        Latitude = latitude;
        Longitude = longitude;
        SensorType = sensorType;
        Status = SensorStatus.Active;
        InstalledAt = installedAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateLocation(decimal latitude, decimal longitude, string placeName)
    {
        if (string.IsNullOrWhiteSpace(placeName))
            throw new ArgumentException("Place name is required.");

        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude.");

        Latitude = latitude;
        Longitude = longitude;
        PlaceName = placeName.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeStatus(SensorStatus status)
    {
        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
