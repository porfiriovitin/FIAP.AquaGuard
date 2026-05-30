using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.AquaGuard.Application.Features.Auth.Register;

public class RegisterUserAccountUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasherProvider _passwordHasher;
    private readonly IValidator<RequestRegisterUser> _validator;

    public RegisterUserAccountUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasherProvider passwordHasher, IValidator<RequestRegisterUser> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<ResponseRegisteredUser> ExecuteAsync(RequestRegisterUser request, Guid RequestedByUserId, UserRole? RequestedByRole = null)
    {
        /// :: Validate the request.
        Validate(request, _validator);

        /// :: Verifies if the user already exists.
        var userExists = await _userRepository.GetByEmailAsync(request.Email);

        if (userExists is not null)
            throw new ErrorOnValidationException("Email já cadastrado.");

        /// :: Hashes the password.
        var hashedPassword = _passwordHasher.CreatePasswordHash(request.Password);

        var role = UserRole.Employee;
        Guid? cityId = null;

        if (request.role.HasValue)
        {
            if (RequestedByRole != UserRole.Admin)
                throw new ErrorOnValidationException("Somente admin pode definir o role no registro.");

            role = request.role.Value;
        }

        if (RequestedByRole == UserRole.Manager)
        {
            if (RequestedByUserId == Guid.Empty)
                throw new ErrorOnValidationException("Usuario solicitante invalido.");

            var manager = await _userRepository.GetByIdAsync(RequestedByUserId);

            if (manager is null)
                throw new ErrorOnValidationException("Manager nao encontrado.");

            if (manager.CityId is null)
                throw new ErrorOnValidationException("Manager sem cidade vinculada.");

            cityId = manager.CityId;
        }

        /// :: Maps the request to the domain model.
        User newUser = new(
            name: request.Name,
            email: request.Email.Trim(),
            passwordHash: hashedPassword,
            role: role,
            cityId: cityId);

        /// :: Save the user to the database.
        await _userRepository.AddAsync(newUser);

        await _unitOfWork.CommitAsync();

        return new ResponseRegisteredUser(
            Name: newUser.Name,
            Email: newUser.Email
        );
        
    }

    private static void Validate(RequestRegisterUser request, IValidator<RequestRegisterUser> validator)
    {
        var result = validator.Validate(request);

        /// :: If the validation fails, throw an exception with the error messages.
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));

    }

}
