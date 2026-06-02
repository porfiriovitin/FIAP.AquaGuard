using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.UpdateCityPaidPlan;

public class RequestUpdateCityPaidPlanValidator : AbstractValidator<RequestUpdateCityPaidPlan>
{
    public RequestUpdateCityPaidPlanValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("Id da cidade e obrigatorio.");

        RuleFor(x => x.IsPaidPlan)
            .Must(x => x is 0 or 1)
            .WithMessage("IsPaidPlan deve ser 0 ou 1.");
    }
}
