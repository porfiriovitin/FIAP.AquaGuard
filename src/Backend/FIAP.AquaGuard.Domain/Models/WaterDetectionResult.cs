namespace FIAP.AquaGuard.Domain.Models;

public record WaterDetectionResult(
    Coordinates Coordinates,
    double WaterPercentage,
    string? ImageData,
    long AmountOfImagesFound,
    string DataSource = "sentinel-1"
    );

