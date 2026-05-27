using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginUseCase _useCase;

    public LoginController(LoginUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PayloadResponse<ResponseLoginDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLogin request)
    {
        ResponseLogin result = await _useCase.ExecuteAsync(request);

        ResponseLoginDTO responseDTO = new(result.Name, result.Email, result.Role);

        Response.Cookies.Append("access_token", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            Path = "/"
        });

        return Ok(new PayloadResponse<ResponseLoginDTO> {Status=nameof(ResponseStatus.Success), Message= "Login Realizado com sucesso" ,Data = responseDTO });
    }
 
}
