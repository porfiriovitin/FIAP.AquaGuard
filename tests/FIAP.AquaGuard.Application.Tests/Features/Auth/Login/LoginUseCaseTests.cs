using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Domain.Services;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentAssertions;
using Moq;

namespace FIAP.AquaGuard.Application.Tests.Features.Auth.Login;

public class LoginUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnResponseWithToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var tokenProvider = new Mock<ITokenProvider>();
        var passwordHasher = new Mock<IPasswordHasherProvider>();
        var validator = new RequestLoginValidator();

        var request = new RequestLogin("user@email.com", "123456");
        var user = new User("User", "user@email.com", "hashed");

        userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        passwordHasher.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);
        tokenProvider.Setup(x => x.GenerateToken(user)).Returns("jwt-token");

        var useCase = new LoginUseCase(userRepository.Object, tokenProvider.Object, passwordHasher.Object, validator);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        response.Token.Should().Be("jwt-token");
        response.Email.Should().Be("user@email.com");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowLoginException_WhenPasswordIsInvalid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var tokenProvider = new Mock<ITokenProvider>();
        var passwordHasher = new Mock<IPasswordHasherProvider>();
        var validator = new RequestLoginValidator();

        var request = new RequestLogin("user@email.com", "wrong");
        var user = new User("User", "user@email.com", "hashed");

        userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        passwordHasher.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);

        var useCase = new LoginUseCase(userRepository.Object, tokenProvider.Object, passwordHasher.Object, validator);

        // Act
        Func<Task> act = () => useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<LoginException>();
    }
}
