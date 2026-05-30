using System.Net;
using System.Net.Http;
using System.Text;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FIAP.AquaGuard.Infrastructure.Services.OpenMeteo;
using FIAP.AquaGuard.Infrastructure.Tests.TestDoubles;
using FluentAssertions;

namespace FIAP.AquaGuard.Infrastructure.Tests.Services.OpenMeteo;

public class OpenMeteoServiceTests
{
    [Fact]
    public async Task GetRainForecast_ShouldReturnAggregatedValues_WhenApiResponseIsValid()
    {
        // Arrange
        var json = """
                   {
                     "hourly": {
                       "time": ["t1", "t2", "t3"],
                       "precipitation": [10.5, 20.0, 5.25]
                     }
                   }
                   """;

        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var httpClient = new HttpClient(handler);
        var service = new OpenMeteoService(httpClient);
        var coordinates = new Coordinates(-23.55, -46.63);

        // Act
        var result = await service.GetRainForecast(coordinates);

        // Assert
        result.Rain24hMm.Should().Be(35.75);
        result.Rain7dMm.Should().Be(35.75);
    }

    [Fact]
    public async Task GetElevation_ShouldThrowOpenMeteoException_WhenApiResponseIsNotSuccess()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest));
        var httpClient = new HttpClient(handler);
        var service = new OpenMeteoService(httpClient);
        var coordinates = new Coordinates(-23.55, -46.63);

        // Act
        Func<Task> act = () => service.GetElevation(coordinates);

        // Assert
        await act.Should().ThrowAsync<OpenMeteoException>();
    }
}
