using FIAP.AquaGuard.Domain.Models;

namespace FIAP.Aquaguard.Application.Features.Risks.Shared;

internal static class RiskResponseMapper
{
    public static ResponseRiskSimple ToSimple(FloodRiskResult risk)
    {
        return new ResponseRiskSimple(
            Latitude: risk.Coordinates.Latitude,
            Longitude: risk.Coordinates.Longitude,
            ForecastRain24hMm: risk.ForecastRain24hMm,
            AccumulatedRain7dMm: risk.AccumulatedRain7dMm,
            CurrentOrForecastRiverFlow: risk.CurrentOrForecastRiverFlow,
            MaxRiverFlowNextDays: risk.MaxRiverFlowNextDays,
            TerrainElevationMeters: risk.TerrainElevationMeters,
            WaterDetectedPercentage: risk.WaterDetectedPercentage,
            RiskScore: risk.RiskScore,
            RiskLevel: risk.RiskLevel,
            AlertMessage: risk.AlertMessage,
            Sources: new ResponseRiskSources(
                Weather: risk.Sources.Weather,
                RiverFlow: risk.Sources.RiverFlow,
                Elevation: risk.Sources.Elevation,
                Satellite: risk.Sources.Satellite));
    }

    public static ResponseRiskComplete ToComplete(FloodRiskResultWithSensor risk)
    {
        return new ResponseRiskComplete(
            Latitude: risk.Coordinates.Latitude,
            Longitude: risk.Coordinates.Longitude,
            ForecastRain24hMm: risk.ForecastRain24hMm,
            AccumulatedRain7dMm: risk.AccumulatedRain7dMm,
            CurrentOrForecastRiverFlow: risk.CurrentOrForecastRiverFlow,
            MaxRiverFlowNextDays: risk.MaxRiverFlowNextDays,
            TerrainElevationMeters: risk.TerrainElevationMeters,
            WaterDetectedPercentage: risk.WaterDetectedPercentage,
            Sensors: risk.Sensors.Select(sensor => new ResponseRiskSensor(
                PlaceName:sensor.PlaceName,
                SensorId: sensor.SensorId,
                WaterLevelCm: sensor.WaterLevelCm,
                FlowRateM3s: sensor.FlowRateM3s,
                RainfallMm: sensor.RainfallMm,
                BatteryLevel: sensor.BatteryLevel,
                SignalStrength: sensor.SignalStrength,
                MeasuredAt: sensor.MeasuredAt)).ToList(),
            RiskScore: risk.RiskScore,
            RiskLevel: risk.RiskLevel,
            AlertMessage: risk.AlertMessage,
            Sources: new ResponseRiskSources(
                Weather: risk.Sources.Weather,
                RiverFlow: risk.Sources.RiverFlow,
                Elevation: risk.Sources.Elevation,
                Satellite: risk.Sources.Satellite));
    }
}
