using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.DeleteUser;

public class DeleteUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestDeleteUser> _validator;

    public DeleteUserUseCase(IUserRepository userRepository,IUnitOfWork unitOfWork,IValidator<RequestDeleteUser> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task ExecuteAsync(RequestDeleteUser request)
    {
        /// :: Validates the request.
        Validate(request, _validator);

        /// :: Gets the user on DB.
        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
            throw new ErrorOnValidationException("Usuario nao encontrado.");

        /// :: Manager can only deletes employees with same cityId.
        if (request.RequestedByRole == UserRole.Manager)
        {
            User? requestingUser = await _userRepository.GetByIdAsync(request.RequestedByUserId);

            if (requestingUser is null)
                throw new ErrorOnValidationException("Usuario solicitante nao encontrado.");

            if (user.Role != UserRole.Employee || requestingUser.CityId is null || user.CityId is null || requestingUser.CityId != user.CityId)
                throw new ErrorOnValidationException("Manager so pode excluir conta de funcionario da mesma cidade.");
        }

        _userRepository.Delete(user);
        await _unitOfWork.CommitAsync();
    }

    private static void Validate(RequestDeleteUser request, IValidator<RequestDeleteUser> validator)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
