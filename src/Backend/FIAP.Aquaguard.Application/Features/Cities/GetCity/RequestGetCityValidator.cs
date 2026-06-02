using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.GetCity;

public class RequestGetCityValidator : AbstractValidator<RequestGetCity>
{
    public RequestGetCityValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("Id da cidade e obrigatorio.");
    }
}
