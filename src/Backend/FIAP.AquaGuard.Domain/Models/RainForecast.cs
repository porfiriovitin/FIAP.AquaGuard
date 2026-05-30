namespace FIAP.AquaGuard.Domain.Models;

public record RainForecast(
    Coordinates Coordinates,
    double Rain24hMm,
    double Rain7dMm,
    Hourly Hourly
);

public record Hourly(
    List<string> Time,
    List<double> Precipitation
);
