using FIAP.Aquaguard.Application.UseCases.User.Register;
using FIAP.AquaGuard.Communication.Requests;
using FIAP.AquaGuard.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;

namespace FIAP.AquaGuard.API.Controllers;

[Route("~/api/users")]
[ApiController]
public class UserController : Controller
{
    [HttpPost]
    [Route("~/register")]
    public IActionResult Register([FromBody] RequestRegisterUser request)
    {
        var useCase = new RegisterUserAccountUseCase();

        PayloadResponse<ResponseRegisteredUserJson> result = useCase.Execute(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }

}
