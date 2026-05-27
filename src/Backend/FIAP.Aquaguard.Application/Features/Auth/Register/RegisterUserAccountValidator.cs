using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Exception;
using FluentValidation;

namespace FIAP.AquaGuard.Application.Features.Auth.Register
{
    public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUser>
    {
        public RegisterUserAccountValidator() {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
            RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED).MinimumLength(6).WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH);
            When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
            });
            //RuleFor(user => user.Role).Must(role => role == null || nameof(UserRole.Admin) != role.ToString()).WithMessage(ResourceMessagesException.VALIDATION_ROLE_INVALID);
        }
    }
}
