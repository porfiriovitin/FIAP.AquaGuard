using FIAP.AquaGuard.Application.Features.Auth.Register;
using FIAP.AquaGuard.API.Models;
using Microsoft.AspNetCore.Mvc;

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
        PayloadResponse<ResponseRegisteredUser> result = await _useCase.ExecuteAsync(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }

}
