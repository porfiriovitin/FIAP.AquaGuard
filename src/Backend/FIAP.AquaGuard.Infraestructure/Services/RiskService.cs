using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Models.RiskMessages;
using FIAP.AquaGuard.Domain.Providers;

namespace FIAP.AquaGuard.Infrastructure.Services;

public class RiskService : IRiskProvider
{
    private readonly IOpenMeteoProvider _openMeteoService;
    private readonly ISatelliteProvider _satelliteService;
    private readonly ISensorProvider _sensorService;

    public RiskService( IOpenMeteoProvider openMeteo, ISatelliteProvider satellite, ISensorProvider sensor)
    {
        _openMeteoService = openMeteo;
        _satelliteService = satellite;
        _sensorService = sensor;   
    }

    public async Task<FloodRiskResult> GetFloodRiskAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        /// :: Paralell async calls.
        var rainTask =  _openMeteoService.GetRainForecast(coordinates, cancellationToken);
        var floodTask =  _openMeteoService.GetFloodForecast(coordinates, cancellationToken);
        var elevationTask =  _openMeteoService.GetElevation(coordinates, cancellationToken);
        var waterDetectionTask =  _satelliteService.GetWaterDetectionResult(coordinates);

        /// :: Await all tasks to complete before following with execution.
        await Task.WhenAll(rainTask, floodTask, elevationTask, waterDetectionTask, waterDetectionTask);

        RainForecast rain = await rainTask;
        FloodForecast flood = await floodTask;
        ElevationResult elevation = await elevationTask;
        WaterDetectionResult waterDetection = await waterDetectionTask;

        /// :: Gets the risk score based on the retrieved data, providing a comprehensive assessment of flood risk by considering multiple environmental factors that contribute to flooding potential. The score is calculated by assigning weights to each factor, with higher weights for more critical conditions (e.g., heavy rainfall, high discharge forecasts, low elevation, and high water detection). The final score is capped at 100 to provide a standardized risk assessment.
        var Score = GetRiskScore(rain, flood, elevation, waterDetection);

        /// :: Classifies the risk.
        var level = ClassifyRisk(Score);

        return new FloodRiskResult(
            Coordinates: coordinates,
            ForecastRain24hMm: rain.Rain24hMm,
            AccumulatedRain7dMm: rain.Rain7dMm,
            CurrentOrForecastRiverFlow: flood.CurrentOrForecastedDischarge,
            MaxRiverFlowNextDays: flood.MaximumDischargeNextDays,
            TerrainElevationMeters: elevation.ElevationMeters,
            WaterDetectedPercentage: waterDetection.WaterPercentage,
            RiskScore: (int)Score,
            RiskLevel: level,
            AlertMessage: GetRiskMessage(level),
            Sources: new FloodRiskSources()
        );
    }

    public async Task<FloodRiskResultWithSensor> GetFloodRiskWithSensorAsync(Coordinates coordinates, Guid cityId, CancellationToken cancellationToken = default)
    {
        /// :: Paralell async calls.
        var rainTask = _openMeteoService.GetRainForecast(coordinates, cancellationToken);
        var floodTask = _openMeteoService.GetFloodForecast(coordinates, cancellationToken);
        var elevationTask = _openMeteoService.GetElevation(coordinates, cancellationToken);
        var waterDetectionTask = _satelliteService.GetWaterDetectionResult(coordinates);
        var sensorTask = _sensorService.GetSensorResult(cityId);

        /// :: Await all tasks to complete before following with execution.
        await Task.WhenAll(rainTask, floodTask, elevationTask, waterDetectionTask, sensorTask);

        RainForecast rain = await rainTask;
        FloodForecast flood = await floodTask;
        ElevationResult elevation = await elevationTask;
        WaterDetectionResult waterDetection = await waterDetectionTask;
        List<SensorReadingResult> sensorReadingResults = await sensorTask;

        /// :: Gets the risk score based on the retrieved data, providing a comprehensive assessment of flood risk by considering multiple environmental factors that contribute to flooding potential. The score is calculated by assigning weights to each factor, with higher weights for more critical conditions (e.g., heavy rainfall, high discharge forecasts, low elevation, and high water detection). The final score is capped at 100 to provide a standardized risk assessment. For paying cities, the method consolidates all sensor readings using a weighted aggregation based on sensor confidence (battery/signal) and recency, plus a peak-risk reinforcement to keep sensitivity to critical local spikes.
        var Score = GetRiskScoreWithSensor(rain, flood, elevation, waterDetection, sensorReadingResults);

        /// :: Classifies the risk.
        var level = ClassifyRisk(Score);

        return new FloodRiskResultWithSensor(
            Coordinates: coordinates,
            ForecastRain24hMm: rain.Rain24hMm,
            AccumulatedRain7dMm: rain.Rain7dMm,
            CurrentOrForecastRiverFlow: flood.CurrentOrForecastedDischarge,
            MaxRiverFlowNextDays: flood.MaximumDischargeNextDays,
            TerrainElevationMeters: elevation.ElevationMeters,
            WaterDetectedPercentage: waterDetection.WaterPercentage,
            Sensors: sensorReadingResults,
            RiskScore: (int)Score,
            RiskLevel: level,
            AlertMessage: GetRiskMessage(level),
            Sources: new FloodRiskSources()
        );
    }

    /// <summary>
    /// Gets elevation score based on the elevation in meters. The lower the elevation, the higher the score, indicating a greater risk of flooding.
    /// </summary>
    private static double GetElevationScore(ElevationResult elevationResult)
    {
        var Meters = elevationResult.ElevationMeters ?? null;

        return Meters switch
        {
            null => 0,
            < 20 => 10,
            < 50 => 6,
            < 100 => 3,
            _ => 0,
        };
    }

    /// <summary>
    /// Gets the riskScore for non-paying cities, based on rain forecast, flood forecast, elevation and water detection. The score is calculated by assigning weights to each factor, with higher weights for more critical conditions (e.g., heavy rainfall, high discharge forecasts, low elevation, and high water detection). The final score is capped at 100 to provide a standardized risk assessment. This method allows for a comprehensive evaluation of flood risk by considering multiple environmental factors that contribute to flooding potential.
    /// </summary>
    private static double GetRiskScore(RainForecast rainForecast, FloodForecast floodForecast, ElevationResult elevation, WaterDetectionResult waterDetection)
    {
        var score = 0.0;

        if(rainForecast.Rain24hMm > 50)
        {
            score += 30;
        }

        if(rainForecast.Rain7dMm > 120)
        {
            score += 20;
        }

        if (floodForecast.IsHighDischargeForecasted)
        {
            score += 30;
        }

        if(waterDetection.WaterPercentage > 20)
        {
            score += 20;
        }

        score += GetElevationScore(elevation);

        return Math.Min(score, 100);

    }

    /// <summary>
    /// Gets the risk score for paying cities by consolidating all sensor readings.
    /// Uses a weighted aggregation based on sensor confidence (battery/signal) and recency, plus a peak-risk reinforcement to keep sensitivity to critical local spikes.
    /// </summary>
    private static double GetRiskScoreWithSensor(RainForecast rainForecast, FloodForecast floodForecast, ElevationResult elevation, WaterDetectionResult waterDetection, List<SensorReadingResult> sensorReadingResults)
    {
        /// :: Calculates the base score.
        var baseScore = GetRiskScore(rainForecast, floodForecast, elevation, waterDetection);
        if (sensorReadingResults is null || sensorReadingResults.Count == 0)
            return baseScore;

        /// :: Gets single sensor score.
        static double GetSingleSensorScore(SensorReadingResult sensorReading)
        {
            var score = 0.0;

            if (sensorReading.WaterLevelCm > 350)
                score += 20;
            else if (sensorReading.WaterLevelCm > 250)
                score += 14;
            else if (sensorReading.WaterLevelCm > 150)
                score += 8;

            if (sensorReading.FlowRateM3s > 700)
                score += 15;
            else if (sensorReading.FlowRateM3s > 400)
                score += 10;
            else if (sensorReading.FlowRateM3s > 200)
                score += 5;

            if (sensorReading.RainfallMm > 50)
                score += 10;
            else if (sensorReading.RainfallMm > 20)
                score += 5;

            return score;
        }

        /// :: Get confidence using signal strength and battery level, ensuring that low confidence readings have less influence on the final score. The confidence is clamped between 0.4 and 1.0 to prevent overly penalizing sensors with moderate issues while still reducing the impact of unreliable data.
        static double GetConfidence(SensorReadingResult sensorReading)
        {
            var batteryConfidence = Math.Clamp(sensorReading.BatteryLevel / 100.0, 0.4, 1.0);
            var signalConfidence = Math.Clamp(sensorReading.SignalStrength / 100.0, 0.4, 1.0);
            return (batteryConfidence + signalConfidence) / 2.0;
        }

        /// :: Get recency weight, giving more importance to recent measurements. The weight decreases linearly over 24 hours, with a minimum weight of 0.3 for readings that are 24 hours old or more. This ensures that older sensor data still contributes to the risk assessment but has less influence than recent data, reflecting the dynamic nature of flood risk.
        static double GetRecencyWeight(SensorReadingResult sensorReading, DateTimeOffset now)
        {
            var hoursSinceMeasurement = Math.Max(0.0, (now - sensorReading.MeasuredAt).TotalHours);
            return Math.Clamp(1.0 - (hoursSinceMeasurement / 24.0), 0.3, 1.0);
        }

        var now = DateTimeOffset.UtcNow;
        var weightedSensorScoreSum = 0.0;
        var totalWeight = 0.0;
        var peakSensorScore = 0.0;

        /// :: Iterates through each sensor reading, calculating a weighted score based on the individual sensor score, confidence, and recency. The weighted scores are summed to calculate an average sensor score, while also tracking the peak sensor score to ensure that critical readings have a significant impact on the final risk assessment.
        foreach (var sensorReading in sensorReadingResults)
        {
            var singleScore = GetSingleSensorScore(sensorReading);
            var confidence = GetConfidence(sensorReading);
            var recencyWeight = GetRecencyWeight(sensorReading, now);
            var weight = confidence * recencyWeight;

            weightedSensorScoreSum += singleScore * weight;
            totalWeight += weight;
            peakSensorScore = Math.Max(peakSensorScore, singleScore);
        }

        /// :: Gets the average score and the consolidated score, combining the average sensor score with the peak sensor score to ensure that critical readings have a significant impact on the final risk assessment. The average sensor score is weighted at 70%, while the peak sensor score is weighted at 30%, allowing for a balanced consideration of overall trends and critical spikes in sensor data.
        var averageSensorScore = totalWeight > 0.0 ? weightedSensorScoreSum / totalWeight : 0.0;
        var consolidatedSensorScore = (averageSensorScore * 0.7) + (peakSensorScore * 0.3);

        /// :: Combines the base score with the consolidated sensor score to calculate the final risk score, ensuring that the influence of sensor data is appropriately integrated into the overall risk assessment. The final score is capped at 100 to maintain a standardized risk scale, allowing for consistent interpretation of flood risk levels across different scenarios.
        var finalScore = baseScore + consolidatedSensorScore;
        return Math.Min(finalScore, 100);
    }

    /// <summary>
    ///  Classifies the risk level based on the calculated risk score. The thresholds for each risk level are defined to provide a clear categorization of flood risk, allowing for appropriate response measures to be taken based on the severity of the situation.
    /// </summary>
    private static RiskLevel ClassifyRisk(double score)
    {
        if (score >= 80)
            return RiskLevel.Critical;

        if (score >= 60)
            return RiskLevel.High;

        if (score >= 30)
            return RiskLevel.Moderate;

        return RiskLevel.Low;
    }

    private static string GetRiskMessage(RiskLevel level)
    {
        return level switch
        {
            RiskLevel.Low => ResourceRiskMessages.LOW_RISK,

            RiskLevel.Moderate => ResourceRiskMessages.MODERATE_RISK,

            RiskLevel.High => ResourceRiskMessages.HIGH_RISK,

            RiskLevel.Critical => ResourceRiskMessages.CRITICAL_RISK,

            _ => ResourceRiskMessages.UNKNOWN_RISK
        };
    }

}

