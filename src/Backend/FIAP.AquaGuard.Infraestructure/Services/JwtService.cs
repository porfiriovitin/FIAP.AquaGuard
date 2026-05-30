using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Services;
using FIAP.AquaGuard.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAP.AquaGuard.Infrastructure.Services;

public class JwtService : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Generates JWT token.
    /// </summary>
    public string GenerateToken(User user)
    {
        /// :: Build the claims for the token, including user ID, email, and role.
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        /// :: Create a symmetric security key using the secret key from options.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        /// :: Create signing credentials using the security key and HMAC SHA256 algorithm.
        var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        /// :: Create the JWT token with issuer, audience, claims, expiration, and signing credentials.
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: credentials
        );

        /// :: Returns the token.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
