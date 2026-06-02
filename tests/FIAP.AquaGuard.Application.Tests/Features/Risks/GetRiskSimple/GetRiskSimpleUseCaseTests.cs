using FIAP.Aquaguard.Application.Features.Risks.GetRiskSimple;
using FIAP.Aquaguard.Application.Features.Risks.Shared;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentAssertions;
using Moq;

namespace FIAP.AquaGuard.Application.Tests.Features.Risks.GetRiskSimple;

public class GetRiskSimpleUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedResponse_WhenRequestIsValid()
    {
        var riskProvider = new Mock<IRiskProvider>();
        var validator = new RequestRiskByCoordinatesValidator();
        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(-23.55, -46.63));
        var risk = new FloodRiskResult(
            Coordinates: new Coordinates(-23.55, -46.63),
            ForecastRain24hMm: 70,
            AccumulatedRain7dMm: 180,
            CurrentOrForecastRiverFlow: 500,
            MaxRiverFlowNextDays: 700,
            TerrainElevationMeters: 15,
            WaterDetectedPercentage: 30,
            RiskScore: 82,
            RiskLevel: RiskLevel.Critical,
            AlertMessage: "critical",
            Sources: new FloodRiskSources());

        riskProvider.Setup(x => x.GetFloodRiskAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(risk);

        var useCase = new GetRiskSimpleUseCase(riskProvider.Object, validator);

        var response = await useCase.ExecuteAsync(request);

        response.RiskScore.Should().Be(82);
        response.RiskLevel.Should().Be(RiskLevel.Critical);
        response.Latitude.Should().Be(-23.55);
        response.Longitude.Should().Be(-46.63);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowErrorOnValidationException_WhenCoordinatesAreInvalid()
    {
        var riskProvider = new Mock<IRiskProvider>();
        var validator = new RequestRiskByCoordinatesValidator();
        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(-200, -46.63));
        var useCase = new GetRiskSimpleUseCase(riskProvider.Object, validator);

        Func<Task> act = () => useCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
        riskProvider.Verify(x => x.GetFloodRiskAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
