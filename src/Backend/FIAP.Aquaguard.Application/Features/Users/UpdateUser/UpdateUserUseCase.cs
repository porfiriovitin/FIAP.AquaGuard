using FIAP.Aquaguard.Application.Features.Users.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.UpdateUser;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestUpdateUser> _validator;

    public UpdateUserUseCase(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IValidator<RequestUpdateUser> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseUser> ExecuteAsync(RequestUpdateUser request)
    {
        Validate(request, _validator);

        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
            throw new ErrorOnValidationException("Usuario nao encontrado.");

        if (request.RequestedByRole == UserRole.Manager)
        {
            User? requestingUser = await _userRepository.GetByIdAsync(request.RequestedByUserId);

            if (requestingUser is null)
                throw new ErrorOnValidationException("Usuario solicitante não encontrado.");

            if (user.Role != UserRole.Employee || requestingUser.CityId is null || user.CityId is null || requestingUser.CityId != user.CityId)
                throw new ErrorOnValidationException("Manager só pode alterar conta de funcionario da mesma cidade.");
        }

        var userWithSameEmail = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());

        if (userWithSameEmail is not null && userWithSameEmail.Id != user.Id)
            throw new ErrorOnValidationException("Email ja cadastrado.");

        user.UpdateProfile(request.Name, request.Email);
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return new ResponseUser(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Role: user.Role.ToString());
    }

    private static void Validate(RequestUpdateUser request, IValidator<RequestUpdateUser> validator)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
