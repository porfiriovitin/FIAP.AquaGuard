using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Auth.Login;

public class RequestLoginValidator : AbstractValidator<RequestLogin>
{
    public RequestLoginValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required.");
        When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("Invalid email format.");
        });
    }
}
