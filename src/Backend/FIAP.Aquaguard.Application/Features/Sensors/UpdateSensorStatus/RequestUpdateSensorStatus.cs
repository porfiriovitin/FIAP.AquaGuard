using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensorStatus;

public record RequestUpdateSensorStatus(Guid SensorId, SensorStatus Status);
