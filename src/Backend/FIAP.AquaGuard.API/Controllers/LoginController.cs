using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.API.Infra.Authentication;
using FIAP.AquaGuard.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginUseCase _useCase;
    private readonly IAuthCookieService _authCookieService;

    public LoginController(LoginUseCase useCase, IAuthCookieService authCookieService)
    {
        _useCase = useCase;
        _authCookieService = authCookieService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PayloadResponse<ResponseLoginDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLogin request)
    {
        ResponseLogin result = await _useCase.ExecuteAsync(request);

        _authCookieService.SetAccessToken(result.Token);

        var responseDTO = new ResponseLoginDTO(result.Name,result.Email,result.Role);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseLoginDTO>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Login realizado com sucesso",
            Data = responseDTO
        });
    }

}
