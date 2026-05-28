using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FluentAssertions;

namespace FIAP.AquaGuard.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldNormalizeEmailAndName_WhenInputIsValid()
    {
        // Arrange
        var name = "  Maria Silva ";
        var email = "  MARIA@EMAIL.COM ";
        var passwordHash = "hashed";

        // Act
        var user = new User(name, email, passwordHash);

        // Assert
        user.Name.Should().Be("Maria Silva");
        user.Email.Should().Be("maria@email.com");
        user.Role.Should().Be(UserRole.Resident);
    }

    [Fact]
    public void UpdatePasswordHash_ShouldThrowArgumentException_WhenPasswordHashIsEmpty()
    {
        // Arrange
        var user = new User("Maria", "maria@email.com", "hash");

        // Act
        Action act = () => user.UpdatePasswordHash(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
