using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.AquaGuard.API.Controllers;
using FIAP.AquaGuard.API.Infra.Authentication;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Domain.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP.AquaGuard.API.Tests.Controllers;

public class LoginControllerTests
{
    [Fact]
    public async Task Login_ShouldReturnStatusCode200_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var tokenProvider = new Mock<ITokenProvider>();
        var passwordHasher = new Mock<IPasswordHasherProvider>();
        var validator = new RequestLoginValidator();
        var authCookieService = new Mock<IAuthCookieService>();
        var request = new RequestLogin("user@email.com", "123456");

        var user = new FIAP.AquaGuard.Domain.Entities.User("User", "user@email.com", "hashed");
        userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        passwordHasher.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);
        tokenProvider.Setup(x => x.GenerateToken(user)).Returns("jwt-token");

        var useCase = new LoginUseCase(userRepository.Object, tokenProvider.Object, passwordHasher.Object, validator);
        var controller = new LoginController(useCase, authCookieService.Object);

        // Act
        var result = await controller.Login(request);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(200);
        authCookieService.Verify(x => x.SetAccessToken("jwt-token"), Times.Once);
    }
}
