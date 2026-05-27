using FIAP.Aquaguard.Application.Shared.Responses;
using FIAP.AquaGuard.Application.Shared.Responses;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public class RegisterUserAccountUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasherProvider _passwordHasher;
    private readonly IValidator<RequestRegisterUser> _validator;

    public RegisterUserAccountUseCase(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasherProvider passwordHasher,
        IValidator<RequestRegisterUser> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<PayloadResponse<ResponseRegisteredUser>> ExecuteAsync(RequestRegisterUser request)
    {
        /// :: Validate the request.
        Validate(request, _validator);

        /// :: Verifies if the user already exists.
        var userExists = await _userRepository.GetByEmailAsync(request.Email);

        if (userExists is not null)
            throw new ErrorOnValidationException("Email já cadastrado.");

        /// :: Hashes the password.
        var hashedPassword = _passwordHasher.CreatePasswordHash(request.Password);

        /// :: Maps the request to the domain model.
        User newUser = new(
            name: request.Name,
            email: request.Email,
            passwordHash: hashedPassword,
            role: UserRole.Employee);

        /// :: Save the user to the database.
        await _userRepository.AddAsync(newUser);

        await _unitOfWork.CommitAsync();

        return new PayloadResponse<ResponseRegisteredUser>
        {
            Status = nameof(ResponseStatus.Success),
            Message = ResourceMessagesException.USER_REGISTERED_SUCESSFULLY,
            Data = new ResponseRegisteredUser(request.Name, request.Email)
        };
    }

    private static void Validate(RequestRegisterUser request, IValidator<RequestRegisterUser> validator)
    {
        var result = validator.Validate(request);

        /// :: If the validation fails, throw an exception with the error messages.
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));

    }

}
