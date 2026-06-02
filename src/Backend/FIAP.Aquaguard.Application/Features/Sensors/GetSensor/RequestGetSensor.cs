using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.GetSensor;

public record RequestGetSensor(Guid SensorId, Guid RequestedByUserId, UserRole RequestedByRole);
