using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.UpdateUser;

public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUser>
{
    public RequestUpdateUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Id do usuario e obrigatorio.");

        RuleFor(x => x.RequestedByUserId)
            .NotEmpty()
            .WithMessage("Id do usuario solicitante e obrigatorio.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome e obrigatorio.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email e obrigatorio.");

        When(x => string.IsNullOrWhiteSpace(x.Email) == false, () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email invalido.");
        });
    }
}
