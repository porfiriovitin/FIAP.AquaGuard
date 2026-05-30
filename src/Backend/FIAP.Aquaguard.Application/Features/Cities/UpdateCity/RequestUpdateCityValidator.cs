using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.UpdateCity;

public class RequestUpdateCityValidator : AbstractValidator<RequestUpdateCity>
{
    public RequestUpdateCityValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("Id da cidade e obrigatorio.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude invalida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude invalida.");
    }
}
