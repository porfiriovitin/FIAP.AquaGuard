using FIAP.AquaGuard.Application.Features.Auth.Register;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentAssertions;
using Moq;

namespace FIAP.AquaGuard.Application.Tests.Features.Auth.Register;

public class RegisterUserAccountUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateUserAndCommit_WhenRequestIsValid()
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

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        response.Email.Should().Be("maria@email.com");
        userRepository.Verify(x => x.AddAsync(It.IsAny<FIAP.AquaGuard.Domain.Entities.User>()), Times.Once);
        unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowErrorOnValidationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var passwordHasher = new Mock<IPasswordHasherProvider>();
        var validator = new RegisterUserAccountValidator();

        var request = new RequestRegisterUser("Maria", "maria@email.com", "123456", null, null);
        var existingUser = new FIAP.AquaGuard.Domain.Entities.User("Other", "maria@email.com", "hash");

        userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(existingUser);

        var useCase = new RegisterUserAccountUseCase(
            userRepository.Object,
            unitOfWork.Object,
            passwordHasher.Object,
            validator);

        // Act
        Func<Task> act = () => useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<ErrorOnValidationException>();
        unitOfWork.Verify(x => x.CommitAsync(), Times.Never);
    }
}
