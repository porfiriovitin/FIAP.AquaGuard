using FIAP.Aquaguard.Application.Abstractions.Authentication;
using FIAP.AquaGuard.Domain.Enums;
using System.Security.Claims;

namespace FIAP.AquaGuard.API.Infra.Authentication;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.Parse(value!);
        }
    }

    public UserRole Role
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Role);

            return Enum.Parse<UserRole>(value!);
        }
    }
}
