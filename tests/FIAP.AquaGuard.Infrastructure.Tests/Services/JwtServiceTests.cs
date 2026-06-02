using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Infrastructure.Options;
using FIAP.AquaGuard.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAP.AquaGuard.Infrastructure.Tests.Services;

public class JwtServiceTests
{
    [Fact]
    public void GenerateToken_ShouldContainExpectedClaims()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new JwtOptions
        {
            SecretKey = "this-is-a-very-long-test-secret-key-for-jwt",
            Issuer = "aquaguard-tests",
            Audience = "aquaguard-api",
            ExpirationMinutes = 30
        });

        var service = new JwtService(options);
        var user = new User("Maria", "maria@email.com", "hash", UserRole.Admin);

        var token = service.GenerateToken(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "aquaguard-tests",
            ValidateAudience = true,
            ValidAudience = "aquaguard-api",
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey))
        }, out _);

        principal.FindFirstValue(ClaimTypes.Email).Should().Be(user.Email);
        principal.FindFirstValue(ClaimTypes.Role).Should().Be(UserRole.Admin.ToString());
        principal.FindFirstValue(ClaimTypes.NameIdentifier).Should().Be(user.Id.ToString());
    }
}
