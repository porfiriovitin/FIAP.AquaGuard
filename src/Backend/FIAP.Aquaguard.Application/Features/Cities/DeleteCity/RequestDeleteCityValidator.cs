using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.DeleteCity;

public class RequestDeleteCityValidator : AbstractValidator<RequestDeleteCity>
{
    public RequestDeleteCityValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("Id da cidade e obrigatorio.");
    }
}
