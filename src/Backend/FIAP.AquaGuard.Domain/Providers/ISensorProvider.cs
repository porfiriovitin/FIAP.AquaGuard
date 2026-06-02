using FIAP.AquaGuard.Domain.Models;

namespace FIAP.AquaGuard.Domain.Providers;

public interface ISensorProvider
{
    Task<List<SensorReadingResult>> GetSensorResult(Guid cityId);
}
