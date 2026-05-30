using FIAP.AquaGuard.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Mvc;
using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly RegisterUserAccountUseCase _useCase;

    public UserController(RegisterUserAccountUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseRegisteredUser>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUser request)
    {
        ResponseRegisteredUser result = await _useCase.ExecuteAsync(request);

        return StatusCode(StatusCodes.Status201Created, new PayloadResponse<ResponseRegisteredUser> { 
            Status = nameof(ResponseStatus.Success), 
            Message = "Usuário registrado com sucesso.", 
            Data = result 
        });
    }

}
