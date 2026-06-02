using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.GetUser;

public class RequestGetUserValidator : AbstractValidator<RequestGetUser>
{
    public RequestGetUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Id do usuário é obrigatório.");
    }
}
