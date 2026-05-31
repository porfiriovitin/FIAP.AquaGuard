using FIAP.AquaGuard.Infrastructure.Services;
using FluentAssertions;

namespace FIAP.AquaGuard.Infrastructure.Tests.Services;

public class BCryptServiceTests
{
    [Fact]
    public void CreatePasswordHash_AndVerifyPassword_ShouldReturnTrueForMatchingPassword()
    {
        var service = new BCryptService();
        const string password = "my-secret-password";

        var hash = service.CreatePasswordHash(password);
        var result = service.VerifyPassword(password, hash);

        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        var service = new BCryptService();
        var hash = service.CreatePasswordHash("password-1");

        var result = service.VerifyPassword("password-2", hash);

        result.Should().BeFalse();
    }
}
