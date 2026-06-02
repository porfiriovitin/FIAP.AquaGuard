using FIAP.Aquaguard.Application.Features.Risks.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;
using EnumRiskLevel = FIAP.AquaGuard.Domain.Enums.RiskLevel;
using ModelRiskLevel = FIAP.AquaGuard.Domain.Models.RiskLevel;

namespace FIAP.Aquaguard.Application.Features.Risks.GetRiskByCoordinates;

public class GetRiskByCoordinatesUseCase
{
    private readonly ICityRepository _cityRepository;
    private readonly IRiskProvider _riskProvider;
    private readonly IRiskAnalysisRepository _riskAnalysisRepository;
    private readonly ISensorReadingRepository _sensorReadingRepository;
    private readonly IRiskAnalysisSensorReadingRepository _riskAnalysisSensorReadingRepository;
    private readonly IRiskDataSourceRepository _riskDataSourceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestRiskByCoordinates> _validator;

    public GetRiskByCoordinatesUseCase(
        ICityRepository cityRepository,
        IRiskProvider riskProvider,
        IRiskAnalysisRepository riskAnalysisRepository,
        ISensorReadingRepository sensorReadingRepository,
        IRiskAnalysisSensorReadingRepository riskAnalysisSensorReadingRepository,
        IRiskDataSourceRepository riskDataSourceRepository,
        IUnitOfWork unitOfWork,
        IValidator<RequestRiskByCoordinates> validator)
    {
        _cityRepository = cityRepository;
        _riskProvider = riskProvider;
        _riskAnalysisRepository = riskAnalysisRepository;
        _sensorReadingRepository = sensorReadingRepository;
        _riskAnalysisSensorReadingRepository = riskAnalysisSensorReadingRepository;
        _riskDataSourceRepository = riskDataSourceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseRiskByCoordinates> ExecuteAsync(RequestRiskByCoordinates request, CancellationToken cancellationToken = default)
    {
        Validate(request, _validator);

        var coordinates = new Coordinates(request.Coordinates.Latitude, request.Coordinates.Longitude);
        var city = await _cityRepository.GetByCoordinates(coordinates);

        if (city is null)
        {
            var riskSimple = await _riskProvider.GetFloodRiskAsync(coordinates, cancellationToken);
            return new ResponseRiskByCoordinates(
                HasCityMatch: false,
                CityId: null,
                SimpleRisk: RiskResponseMapper.ToSimple(riskSimple),
                CompleteRisk: null);
        }

        var riskComplete = await _riskProvider.GetFloodRiskWithSensorAsync(coordinates, city.Id, cancellationToken);

        if (city.IsPaidPlan == 1)
            await PersistPaidCityRiskAsync(city.Id, riskComplete);

        return new ResponseRiskByCoordinates(
            HasCityMatch: true,
            CityId: city.Id,
            SimpleRisk: null,
            CompleteRisk: RiskResponseMapper.ToComplete(riskComplete));
    }

    private static void Validate(RequestRiskByCoordinates request, IValidator<RequestRiskByCoordinates> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

    private async Task PersistPaidCityRiskAsync(Guid cityId, FloodRiskResultWithSensor riskComplete)
    {
        var riskAnalysis = new RiskAnalysis(cityId, DateTimeOffset.UtcNow);

        riskAnalysis.UpdateHydrology(
            forecastRain24hMm: ConvertToDecimal(riskComplete.ForecastRain24hMm),
            accumulatedRain7dMm: ConvertToDecimal(riskComplete.AccumulatedRain7dMm),
            currentOrForecastFlow: ConvertToDecimal(riskComplete.CurrentOrForecastRiverFlow),
            maxFlowNextDays: ConvertToDecimal(riskComplete.MaxRiverFlowNextDays),
            terrainElevationMeters: ConvertToDecimal(riskComplete.TerrainElevationMeters),
            detectedWaterPercentage: ConvertToDecimal(riskComplete.WaterDetectedPercentage));

        riskAnalysis.Classify(riskComplete.RiskScore, MapRiskLevel(riskComplete.RiskLevel), riskComplete.AlertMessage);

        await _riskAnalysisRepository.AddAsync(riskAnalysis);

        foreach (var sensorResult in riskComplete.Sensors)
        {
            var sensorReading = new SensorReading(
                sensorId: sensorResult.SensorId,
                measuredAt: sensorResult.MeasuredAt,
                waterLevelCm: ConvertToDecimal(sensorResult.WaterLevelCm),
                flowRateM3s: ConvertToDecimal(sensorResult.FlowRateM3s),
                rainfallMm: ConvertToDecimal(sensorResult.RainfallMm),
                batteryLevel: ConvertToDecimal(sensorResult.BatteryLevel),
                signalStrength: ConvertToDecimal(sensorResult.SignalStrength));

            await _sensorReadingRepository.AddAsync(sensorReading);
            await _riskAnalysisSensorReadingRepository.AddAsync(new RiskAnalysisSensorReading(riskAnalysis.Id, sensorReading.Id));
        }

        await _riskDataSourceRepository.AddAsync(new RiskDataSource(riskAnalysis.Id, RiskSourceType.Weather, riskComplete.Sources.Weather, DateTimeOffset.UtcNow));
        await _riskDataSourceRepository.AddAsync(new RiskDataSource(riskAnalysis.Id, RiskSourceType.Flow, riskComplete.Sources.RiverFlow, DateTimeOffset.UtcNow));
        await _riskDataSourceRepository.AddAsync(new RiskDataSource(riskAnalysis.Id, RiskSourceType.Elevation, riskComplete.Sources.Elevation, DateTimeOffset.UtcNow));
        await _riskDataSourceRepository.AddAsync(new RiskDataSource(riskAnalysis.Id, RiskSourceType.Satellite, riskComplete.Sources.Satellite, DateTimeOffset.UtcNow));
        await _riskDataSourceRepository.AddAsync(new RiskDataSource(riskAnalysis.Id, RiskSourceType.Sensor, "city-sensors", DateTimeOffset.UtcNow));

        await _unitOfWork.CommitAsync();
    }

    private static decimal? ConvertToDecimal(double? value)
    {
        if (value is null)
            return null;

        return Convert.ToDecimal(value.Value);
    }

    private static EnumRiskLevel MapRiskLevel(ModelRiskLevel riskLevel)
    {
        return riskLevel switch
        {
            ModelRiskLevel.Low => EnumRiskLevel.Low,
            ModelRiskLevel.Moderate => EnumRiskLevel.Moderate,
            ModelRiskLevel.High => EnumRiskLevel.High,
            ModelRiskLevel.Critical => EnumRiskLevel.Critical,
            _ => EnumRiskLevel.Low
        };
    }
}
