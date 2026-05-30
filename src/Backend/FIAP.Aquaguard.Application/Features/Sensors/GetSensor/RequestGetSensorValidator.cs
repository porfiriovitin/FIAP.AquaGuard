using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.GetSensor;

public class RequestGetSensorValidator : AbstractValidator<RequestGetSensor>
{
    public RequestGetSensorValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty()
            .WithMessage("Id do sensor e obrigatorio.");

        RuleFor(x => x.RequestedByUserId)
            .NotEmpty()
            .WithMessage("Id do usuario solicitante e obrigatorio.");
    }
}
