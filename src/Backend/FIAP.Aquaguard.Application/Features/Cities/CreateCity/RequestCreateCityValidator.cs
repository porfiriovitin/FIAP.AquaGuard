using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.CreateCity;

public class RequestCreateCityValidator : AbstractValidator<RequestCreateCity>
{
    public RequestCreateCityValidator()
    {
        RuleFor(x => x.Zipcode)
            .NotEmpty()
            .WithMessage("CEP e obrigatorio.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome da cidade e obrigatorio.");

        RuleFor(x => x.State)
            .NotEmpty()
            .Length(2)
            .WithMessage("Estado deve ter 2 caracteres.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude invalida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude invalida.");
    }
}
