using FIAP.Aquaguard.Application.Features.Users.DeleteUser;
using FIAP.Aquaguard.Application.Features.Users.GetUser;
using FIAP.Aquaguard.Application.Features.Users.Shared;
using FIAP.Aquaguard.Application.Features.Users.UpdateUser;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAP.Aquaguard.Application.Abstractions.Authentication;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly RegisterUserAccountUseCase _registerUserUseCase;
    private readonly GetUserUseCase _getUserUseCase;
    private readonly UpdateUserUseCase _updateUserUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly ICurrentUser _currentUser;

    public UserController(RegisterUserAccountUseCase registerUserUseCase, GetUserUseCase getUserUseCase, UpdateUserUseCase updateUserUseCase, DeleteUserUseCase deleteUserUseCase, ICurrentUser currentUser)
    {
        _registerUserUseCase = registerUserUseCase;
        _getUserUseCase = getUserUseCase;
        _updateUserUseCase = updateUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _currentUser = currentUser;
    }


    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost("register")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseRegisteredUser>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUser request)
    {
        ResponseRegisteredUser result = await _registerUserUseCase.ExecuteAsync(request, _currentUser.UserId, _currentUser.Role);

        return StatusCode(StatusCodes.Status201Created, new PayloadResponse<ResponseRegisteredUser>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Usuário registrado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        ResponseUser result = await _getUserUseCase.ExecuteAsync(new RequestGetUser(id));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseUser>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Usuário encontrado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet]
    [ProducesResponseType(typeof(PayloadResponse<ResponseUsers>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsers([FromQuery] Guid? cityId, int page = 1, int pageSize = 10)
    {
        ResponseUsers result = await _getUserUseCase.ListUsersAsync(_currentUser.UserId, cityId ?? Guid.Empty, _currentUser.Role, page, pageSize);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseUsers>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Usuários encontrados com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] RequestUpdateUserInput request)
    {
        ResponseUser result = await _updateUserUseCase.ExecuteAsync(
            new RequestUpdateUser(id, request.Name, request.Email, _currentUser.UserId, _currentUser.Role));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseUser>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Usuário atualizado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await _deleteUserUseCase.ExecuteAsync(new RequestDeleteUser(id, _currentUser.UserId, _currentUser.Role));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<object>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Usuário removido com sucesso.",
            Data = null
        });
    }

    public record RequestUpdateUserInput(string Name, string Email);
}
