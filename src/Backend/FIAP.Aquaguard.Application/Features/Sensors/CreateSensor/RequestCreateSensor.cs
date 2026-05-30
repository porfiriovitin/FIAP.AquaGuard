using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.CreateSensor;

public record RequestCreateSensor(
    Guid CityId,
    string PlaceName,
    decimal Latitude,
    decimal Longitude,
    SensorType SensorType,
    DateTimeOffset InstalledAt,
    Guid RequestedByUserId,
    UserRole RequestedByRole);

public record RequestCreateSensorInput(
       Guid CityId,
       string PlaceName,
       decimal Latitude,
       decimal Longitude,
       SensorType SensorType,
       DateTimeOffset InstalledAt);