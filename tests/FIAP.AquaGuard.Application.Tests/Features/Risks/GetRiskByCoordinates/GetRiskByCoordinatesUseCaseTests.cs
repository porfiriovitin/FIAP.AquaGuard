using FIAP.Aquaguard.Application.Features.Risks.GetRiskByCoordinates;
using FIAP.Aquaguard.Application.Features.Risks.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FIAP.AquaGuard.Application.Tests.Features.Risks.GetRiskByCoordinates;

public class GetRiskByCoordinatesUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnSimpleRisk_WhenCityIsNotFound()
    {
        var cityRepository = new Mock<ICityRepository>();
        var riskProvider = new Mock<IRiskProvider>();
        var riskAnalysisRepository = new Mock<IRiskAnalysisRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        var riskAnalysisSensorReadingRepository = new Mock<IRiskAnalysisSensorReadingRepository>();
        var riskDataSourceRepository = new Mock<IRiskDataSourceRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new RequestRiskByCoordinatesValidator();

        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(-23.55, -46.63));
        var simpleRisk = new FloodRiskResult(
            new Coordinates(-23.55, -46.63), 10, 20, 100, 200, 80, 5, 25, RiskLevel.Low, "low", new FloodRiskSources());

        cityRepository.Setup(x => x.GetByCoordinates(It.IsAny<Coordinates>())).ReturnsAsync((City?)null);
        riskProvider.Setup(x => x.GetFloodRiskAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>())).ReturnsAsync(simpleRisk);

        var useCase = new GetRiskByCoordinatesUseCase(
            cityRepository.Object,
            riskProvider.Object,
            riskAnalysisRepository.Object,
            sensorReadingRepository.Object,
            riskAnalysisSensorReadingRepository.Object,
            riskDataSourceRepository.Object,
            unitOfWork.Object,
            validator);

        var response = await useCase.ExecuteAsync(request);

        response.HasCityMatch.Should().BeFalse();
        response.SimpleRisk.Should().NotBeNull();
        response.CompleteRisk.Should().BeNull();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPersistRiskData_WhenCityIsPaidPlan()
    {
        var cityRepository = new Mock<ICityRepository>();
        var riskProvider = new Mock<IRiskProvider>();
        var riskAnalysisRepository = new Mock<IRiskAnalysisRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        var riskAnalysisSensorReadingRepository = new Mock<IRiskAnalysisSensorReadingRepository>();
        var riskDataSourceRepository = new Mock<IRiskDataSourceRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var validator = new RequestRiskByCoordinatesValidator();

        var city = new City("01001000", "Sao Paulo", "SP", -23.55m, -46.63m);
        typeof(City).GetProperty("Id")!.SetValue(city, Guid.NewGuid());
        city.BecomePaidPlan();
        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(-23.55, -46.63));
        var sensorReadings = new List<SensorReadingResult>
        {
            new(Guid.NewGuid(), "Marginal", 210, 430, 25, 87, 75, DateTimeOffset.UtcNow)
        };

        var completeRisk = new FloodRiskResultWithSensor(
            Coordinates: new Coordinates(-23.55, -46.63),
            ForecastRain24hMm: 80,
            AccumulatedRain7dMm: 190,
            CurrentOrForecastRiverFlow: 410,
            MaxRiverFlowNextDays: 730,
            TerrainElevationMeters: 12,
            WaterDetectedPercentage: 33,
            Sensors: sensorReadings,
            RiskScore: 88,
            RiskLevel: RiskLevel.Critical,
            AlertMessage: "critical",
            Sources: new FloodRiskSources());

        cityRepository.Setup(x => x.GetByCoordinates(It.IsAny<Coordinates>())).ReturnsAsync(city);
        riskProvider.Setup(x => x.GetFloodRiskWithSensorAsync(It.IsAny<Coordinates>(), city.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(completeRisk);

        var useCase = new GetRiskByCoordinatesUseCase(
            cityRepository.Object,
            riskProvider.Object,
            riskAnalysisRepository.Object,
            sensorReadingRepository.Object,
            riskAnalysisSensorReadingRepository.Object,
            riskDataSourceRepository.Object,
            unitOfWork.Object,
            validator);

        var response = await useCase.ExecuteAsync(request);

        response.HasCityMatch.Should().BeTrue();
        response.CompleteRisk.Should().NotBeNull();
        riskAnalysisRepository.Verify(x => x.AddAsync(It.IsAny<RiskAnalysis>()), Times.Once);
        sensorReadingRepository.Verify(x => x.AddAsync(It.IsAny<SensorReading>()), Times.Once);
        riskAnalysisSensorReadingRepository.Verify(x => x.AddAsync(It.IsAny<RiskAnalysisSensorReading>()), Times.Once);
        riskDataSourceRepository.Verify(x => x.AddAsync(It.IsAny<RiskDataSource>()), Times.Exactly(5));
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }
}
