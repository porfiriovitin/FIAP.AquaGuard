using FIAP.AquaGuard.Domain.Entities;
using FluentAssertions;

namespace FIAP.AquaGuard.Domain.Tests.Entities;

public class CityTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenLatitudeIsInvalid()
    {
        // Arrange
        var invalidLatitude = 120m;

        // Act
        Action act = () => new City("12345-678", "Sao Paulo", "SP", invalidLatitude, -46.63m);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void BecomePaidPlan_ShouldSetPaidFlag_WhenCalled()
    {
        // Arrange
        var city = new City("12345-678", "Sao Paulo", "SP", -23.55m, -46.63m);

        // Act
        city.BecomePaidPlan();

        // Assert
        city.IsPaidPlan.Should().Be(1);
    }
}
