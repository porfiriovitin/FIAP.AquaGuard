using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.CreateSensor;

public class RequestCreateSensorValidator : AbstractValidator<RequestCreateSensor>
{
    public RequestCreateSensorValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("Id da cidade e obrigatorio.");

        RuleFor(x => x.RequestedByUserId)
            .NotEmpty()
            .WithMessage("Id do usuario solicitante e obrigatorio.");

        RuleFor(x => x.PlaceName)
            .NotEmpty()
            .WithMessage("Nome do local e obrigatorio.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude invalida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude invalida.");
    }
}
