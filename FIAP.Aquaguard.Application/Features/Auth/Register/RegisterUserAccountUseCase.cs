using FIAP.AquaGuard.API.Models;
using FIAP.AquaGuard.Application.Features.Auth.Register;
using FIAP.AquaGuard.Application.Shared.Responses;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;

namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public class RegisterUserAccountUseCase
{
    private readonly IUserRepository _userRepository;

    public RegisterUserAccountUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PayloadResponse<ResponseRegisteredUser>> ExecuteAsync(RequestRegisterUser request)
    {
        /// :: Validate the request.
        Validate(request);

        /// :: Verifies if the user already exists.
        var userExists = await _userRepository.GetByEmailAsync(request.Email);

        if (userExists is not null)
            throw new ErrorOnValidationException("Email já cadastrado.");


        /// :: Maps the request to the domain model.
        User newUser = new()
        {
            Name = request.Name,
            Email = request.Email,
            Role = UserRole.Employee,
        };

        /// :: Hashes the password.
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        newUser.PasswordHash = hashedPassword;

        /// :: Save the user to the database.
        await _userRepository.AddAsync(newUser);

        return new PayloadResponse<ResponseRegisteredUser>
        {
            Status = nameof(ResponseStatus.Success),
            Message = ResourceMessagesException.USER_REGISTERED_SUCESSFULLY,
            Data = new ResponseRegisteredUser(request.Name, request.Email)
        };
    }

    private static void Validate(RequestRegisterUser request)
    {
        /// :: Validate the request using FluentValidation.
        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        /// :: If the validation fails, throw an exception with the error messages.
        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
        }

    }

}
