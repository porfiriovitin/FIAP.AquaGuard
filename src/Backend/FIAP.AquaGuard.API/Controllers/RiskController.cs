using FIAP.Aquaguard.Application.Features.Risks.GetRiskByCoordinates;
using FIAP.Aquaguard.Application.Features.Risks.GetRiskSimple;
using FIAP.Aquaguard.Application.Features.Risks.Shared;
using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.AquaGuard.API.Controllers;

[Route("api/risk")]
[ApiController]
public class RiskController : ControllerBase
{
    private readonly GetRiskSimpleUseCase _getRiskSimpleUseCase;
    private readonly GetRiskByCoordinatesUseCase _getRiskByCoordinatesUseCase;

    public RiskController(GetRiskSimpleUseCase getRiskSimpleUseCase, GetRiskByCoordinatesUseCase getRiskByCoordinatesUseCase)
    {
        _getRiskSimpleUseCase = getRiskSimpleUseCase;
        _getRiskByCoordinatesUseCase = getRiskByCoordinatesUseCase;
    }

    [HttpGet("simple")]
    [ProducesResponseType(typeof(PayloadResponse<ResponseRiskSimple>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSimpleRisk([FromQuery] RequestRiskQueryInput query, CancellationToken cancellationToken)
    {
        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(query.Latitude, query.Longitude));
        var result = await _getRiskSimpleUseCase.ExecuteAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseRiskSimple>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Consulta de risco simples realizada com sucesso.",
            Data = result
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PayloadResponse<ResponseRiskByCoordinates>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PayloadResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRiskByCoordinates([FromQuery] RequestRiskQueryInput query, CancellationToken cancellationToken)
    {
        var request = new RequestRiskByCoordinates(new RequestRiskCoordinatesInput(query.Latitude, query.Longitude));
        var result = await _getRiskByCoordinatesUseCase.ExecuteAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, new PayloadResponse<ResponseRiskByCoordinates>
        {
            Status = nameof(ResponseStatus.Success),
            Message = "Consulta de risco realizada com sucesso.",
            Data = result
        });
    }

    public record RequestRiskQueryInput(double Latitude, double Longitude);
}
