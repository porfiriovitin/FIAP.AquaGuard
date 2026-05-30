using FIAP.Aquaguard.Application.Features.Cities.CreateCity;
using FIAP.Aquaguard.Application.Features.Cities.DeleteCity;
using FIAP.Aquaguard.Application.Features.Cities.GetCity;
using FIAP.Aquaguard.Application.Features.Cities.Shared;
using FIAP.Aquaguard.Application.Features.Cities.UpdateCity;
using FIAP.Aquaguard.Application.Features.Cities.UpdateCityPaidPlan;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/cities")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly CreateCityUseCase _createCityUseCase;
    private readonly GetCityUseCase _getCityUseCase;
    private readonly UpdateCityUseCase _updateCityUseCase;
    private readonly DeleteCityUseCase _deleteCityUseCase;
    private readonly UpdateCityPaidPlanUseCase _updateCityPaidPlanUseCase;

    public CityController(CreateCityUseCase createCityUseCase, GetCityUseCase getCityUseCase, UpdateCityUseCase updateCityUseCase, DeleteCityUseCase deleteCityUseCase ,UpdateCityPaidPlanUseCase updateCityPaidPlanUseCase)
    {
        _createCityUseCase = createCityUseCase;
        _getCityUseCase = getCityUseCase;
        _updateCityUseCase = updateCityUseCase;
        _deleteCityUseCase = deleteCityUseCase;
        _updateCityPaidPlanUseCase = updateCityPaidPlanUseCase;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ProducesResponseType(typeof(PayloadResponse<ResponseCity>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RequestCreateCity request)
    {
        ResponseCity result = await _createCityUseCase.ExecuteAsync(request);

        return StatusCode(StatusCodes.Status201Created, new PayloadResponse<ResponseCity>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Cidade criada com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseCity>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        ResponseCity result = await _getCityUseCase.ExecuteAsync(new RequestGetCity(id));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseCity>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Cidade encontrada com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("{page:int?}/{pageSize:int?}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseCities>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List([FromRoute] int page = 1, [FromRoute] int pageSize = 10)
    {
        ResponseCities result = await _getCityUseCase.ListCitiesAsync(page, pageSize);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseCities>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Cidades encontradas com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseCity>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateCityInput request)
    {
        ResponseCity result = await _updateCityUseCase.ExecuteAsync(
            new RequestUpdateCity(id, request.Latitude, request.Longitude));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseCity>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Cidade atualizada com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("{id:guid}/paid-plan/{isPaidPlan:int}")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseCity>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePaidPlan([FromRoute] Guid id, [FromRoute] int isPaidPlan)
    {
        ResponseCity result = await _updateCityPaidPlanUseCase.ExecuteAsync(new RequestUpdateCityPaidPlan(id, (short)isPaidPlan));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseCity>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Plano da cidade atualizado com sucesso.",
            Data = result
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _deleteCityUseCase.ExecuteAsync(new RequestDeleteCity(id));

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<object>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Cidade removida com sucesso.",
            Data = null
        });
    }

    public record RequestUpdateCityInput(decimal Latitude, decimal Longitude);
}
