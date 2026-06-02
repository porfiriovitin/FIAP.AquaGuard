using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Risks.Shared;

public class RequestRiskByCoordinatesValidator : AbstractValidator<RequestRiskByCoordinates>
{
    public RequestRiskByCoordinatesValidator()
    {
        RuleFor(x => x.Coordinates)
            .NotNull()
            .WithMessage("Objeto coordinates e obrigatorio.");

        RuleFor(x => x.Coordinates.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude deve estar entre -90 e 90.");

        RuleFor(x => x.Coordinates.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude deve estar entre -180 e 180.");
    }
}
