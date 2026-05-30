using FIAP.Aquaguard.Application.Abstractions.Authentication;
using FIAP.Aquaguard.Application.Features.Sensors.CreateSensor;
using FIAP.Aquaguard.Application.Features.Sensors.DeleteSensor;
using FIAP.Aquaguard.Application.Features.Sensors.GetSensor;
using FIAP.Aquaguard.Application.Features.Sensors.Shared;
using FIAP.Aquaguard.Application.Features.Sensors.UpdateSensor;
using FIAP.Aquaguard.Application.Features.Sensors.UpdateSensorStatus;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using FIAP.AquaGuard.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/sensors")]
[ApiController]
public class SensorController : ControllerBase
{
    private readonly CreateSensorUseCase _createSensorUseCase;
    private readonly GetSensorUseCase _getSensorUseCase;
    private readonly UpdateSensorUseCase _updateSensorUseCase;
    private readonly DeleteSensorUseCase _deleteSensorUseCase;
    private readonly UpdateSensorStatusUseCase _updateSensorStatusUseCase;
    private readonly ICurrentUser _currentUser;

    public SensorController(
        CreateSensorUseCase createSensorUseCase,
        GetSensorUseCase getSensorUseCase,
        UpdateSensorUseCase updateSensorUseCase,
        DeleteSensorUseCase deleteSensorUseCase,
        UpdateSensorStatusUseCase updateSensorStatusUseCase,
        ICurrentUser currentUser)
    {
        _createSensorUseCase = createSensorUseCase;
        _getSensorUseCase = getSensorUseCase;
        _updateSensorUseCase = updateSensorUseCase;
        _deleteSensorUseCase = deleteSensorUseCase;
        _updateSensorStatusUseCase = updateSensorStatusUseCase;
        _currentUser = currentUser;
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPost]
    [ProducesResponseType(typeof(PayloadResponse<ResponseSensor>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RequestCreateSensorInput request)
    {
        var result = await _createSensorUseCase.ExecuteAsync(new RequestCreateSensor(
            request.CityId,
            request.PlaceName,
            request.Latitude,
            request.Longitude,
            request.SensorType,
            request.InstalledAt,
            _currentUser.UserId,
            _currentUser.Role));

        return StatusCode(StatusCodes.Status201Created, new PayloadResponse<ResponseSensor>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Sensor criado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseSensor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _getSensorUseCase.ExecuteAsync(new RequestGetSensor(id, _currentUser.UserId, _currentUser.Role));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseSensor>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Sensor encontrado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpGet("{cityId:guid}/{page:int?}/{pageSize:int?}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseSensors>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List([FromRoute] Guid cityId, [FromRoute] int page = 1, [FromRoute] int pageSize = 10)
    {
        var result = await _getSensorUseCase.ListAsync(cityId, _currentUser.UserId, _currentUser.Role, page, pageSize);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseSensors>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Sensores encontrados com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseSensor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateSensorInput request)
    {
        var result = await _updateSensorUseCase.ExecuteAsync(new RequestUpdateSensor(
            id,
            request.PlaceName,
            request.Latitude,
            request.Longitude,
            _currentUser.UserId,
            _currentUser.Role));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseSensor>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Sensor atualizado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{id:guid}/status/{status:int}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseSensor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromRoute] int status)
    {
        var result = await _updateSensorStatusUseCase.ExecuteAsync(new RequestUpdateSensorStatus(id, (SensorStatus)status));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseSensor>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Status do sensor atualizado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _deleteSensorUseCase.ExecuteAsync(new RequestDeleteSensor(id, _currentUser.UserId, _currentUser.Role));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<object>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Sensor removido com sucesso.",
            Data = null
        });
    }

   


}
