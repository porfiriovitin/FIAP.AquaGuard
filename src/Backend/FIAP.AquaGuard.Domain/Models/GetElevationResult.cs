namespace FIAP.AquaGuard.Domain.Models;

public record ElevationResult(
    Coordinates Coordinates,
    double? ElevationMeters
);
