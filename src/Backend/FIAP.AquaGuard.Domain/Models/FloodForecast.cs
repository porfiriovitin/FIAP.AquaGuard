namespace FIAP.AquaGuard.Domain.Models;

public record FloodForecast(
    Coordinates Coordinates,
    double? CurrentOrForecastedDischarge,
    double? MaximumDischargeNextDays,
    bool IsHighDischargeForecasted,
    string Unit,
    DailyDischarge? Daily
);

public record DailyDischarge(
    List<string> Time,
    List<double> RiverDischarge
);
