using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensorStatus;

public class RequestUpdateSensorStatusValidator : AbstractValidator<RequestUpdateSensorStatus>
{
    public RequestUpdateSensorStatusValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty()
            .WithMessage("Id do sensor e obrigatorio.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status do sensor invalido.");
    }
}
