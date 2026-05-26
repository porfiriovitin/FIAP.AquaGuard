using FIAP.AquaGuard.Communication.Requests;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Exception;
using FluentValidation;

namespace FIAP.Aquaguard.Application.UseCases.User.Register
{
    public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUser>
    {

        private static List<String> Roles = [nameof(UserRole.Employee), nameof(UserRole.Manager)];

        public RegisterUserAccountValidator() {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
            RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED).MinimumLength(6).WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH);
            When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
            });
            RuleFor(user => user.Role).Must(role => Roles.Contains(role.ToString())).WithMessage(ResourceMessagesException.VALIDATION_ROLE_INVALID);
        }
    }
}
