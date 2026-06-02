using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.DeleteSensor;

public class RequestDeleteSensorValidator : AbstractValidator<RequestDeleteSensor>
{
    public RequestDeleteSensorValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty()
            .WithMessage("Id do sensor e obrigatorio.");

        RuleFor(x => x.RequestedByUserId)
            .NotEmpty()
            .WithMessage("Id do usuario solicitante e obrigatorio.");
    }
}
