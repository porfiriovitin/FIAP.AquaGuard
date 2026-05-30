using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensor;

public class RequestUpdateSensorValidator : AbstractValidator<RequestUpdateSensor>
{
    public RequestUpdateSensorValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty()
            .WithMessage("Id do sensor e obrigatorio.");

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
