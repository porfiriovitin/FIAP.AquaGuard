using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.DeleteSensor;

public record RequestDeleteSensor(Guid SensorId, Guid RequestedByUserId, UserRole RequestedByRole);
