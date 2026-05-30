using FIAP.AquaGuard.API.Controllers;
using FIAP.AquaGuard.Application.Features.Auth.Register;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP.AquaGuard.API.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Register_ShouldReturnStatusCode201_WhenRequestIsValid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var passwordHasher = new Mock<IPasswordHasherProvider>();
        var validator = new RegisterUserAccountValidator();
        var request = new RequestRegisterUser("Maria", "maria@email.com", "123456", null, null);

        userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((FIAP.AquaGuard.Domain.Entities.User?)null);
        passwordHasher.Setup(x => x.CreatePasswordHash(request.Password)).Returns("hashed-password");

        var useCase = new RegisterUserAccountUseCase(
            userRepository.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            validator);
        var controller = new UserController(useCase);

        // Act
        var result = await controller.Register(request);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(201);
    }
}
