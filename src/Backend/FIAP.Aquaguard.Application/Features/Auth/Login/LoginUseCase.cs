using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Domain.Services;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Auth.Login;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenService;
    private readonly IPasswordHasherProvider _passwordHasher;
    private readonly IValidator<RequestLogin> _validator;

    public LoginUseCase(
        IUserRepository userRepository,
        ITokenProvider tokenService,
        IPasswordHasherProvider passwordHasher,
        IValidator<RequestLogin> validator)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<ResponseLogin> ExecuteAsync(RequestLogin request)
    {
        /// :: Validates the request
        Validate(request, _validator);

        /// :: Fetch the user from the repository using the email provided in the request.
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null) 
            throw new LoginException(ResourceMessagesException.LOGIN_INVALID);

        /// :: Verify the password provided in the request against the stored password for the user.
        bool validPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!validPassword)
            throw new LoginException(ResourceMessagesException.LOGIN_INVALID);

        /// :: Generates Token
        var jwtToken = _tokenService.GenerateToken(user);

        /// :: Returns a ResponseLogin object containing the user's name, email, role, and the generated JWT token.
        return new ResponseLogin(
            Name: user.Name,
            Email: user.Email,
            Token: jwtToken,
            Role: user.Role.ToString()
        );
    }

    /// <summary>
    ///  Validates the request.
    /// </summary>
    private static void Validate(RequestLogin request, IValidator<RequestLogin> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }

}
