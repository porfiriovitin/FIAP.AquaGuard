using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.DeleteUser;

public class RequestDeleteUserValidator : AbstractValidator<RequestDeleteUser>
{
    public RequestDeleteUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Id do usuario e obrigatorio.");

        RuleFor(x => x.RequestedByUserId)
            .NotEmpty()
            .WithMessage("Id do usuario solicitante e obrigatorio.");
    }
}
