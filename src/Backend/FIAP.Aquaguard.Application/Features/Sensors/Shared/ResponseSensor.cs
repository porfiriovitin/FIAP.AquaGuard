using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.Shared;

public record ResponseSensor(
    Guid Id,
    Guid CityId,
    string PlaceName,
    decimal Latitude,
    decimal Longitude,
    SensorType SensorType,
    SensorStatus Status,
    DateTimeOffset InstalledAt);

public record ResponseSensors(
    long amount,
    long total,
    long page,
    long totalPages,
    List<ResponseSensor> sensors);
