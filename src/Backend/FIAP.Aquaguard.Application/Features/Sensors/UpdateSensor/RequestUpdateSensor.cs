using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensor;

public record RequestUpdateSensor(
    Guid SensorId,
    string PlaceName,
    decimal Latitude,
    decimal Longitude,
    Guid RequestedByUserId,
    UserRole RequestedByRole);

public record RequestUpdateSensorInput(
    string PlaceName,
    decimal Latitude,
    decimal Longitude);
