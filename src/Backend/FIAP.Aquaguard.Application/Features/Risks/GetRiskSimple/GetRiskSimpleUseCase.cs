using FIAP.Aquaguard.Application.Features.Risks.Shared;
using FIAP.AquaGuard.Domain.Models;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Risks.GetRiskSimple;

public class GetRiskSimpleUseCase
{
    private readonly IRiskProvider _riskProvider;
    private readonly IValidator<RequestRiskByCoordinates> _validator;

    public GetRiskSimpleUseCase(IRiskProvider riskProvider, IValidator<RequestRiskByCoordinates> validator)
    {
        _riskProvider = riskProvider;
        _validator = validator;
    }

    public async Task<ResponseRiskSimple> ExecuteAsync(RequestRiskByCoordinates request, CancellationToken cancellationToken = default)
    {
        Validate(request, _validator);

        var coordinates = new Coordinates(request.Coordinates.Latitude, request.Coordinates.Longitude);
        var risk = await _riskProvider.GetFloodRiskAsync(coordinates, cancellationToken);

        return RiskResponseMapper.ToSimple(risk);
    }

    private static void Validate(RequestRiskByCoordinates request, IValidator<RequestRiskByCoordinates> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
