using FIAP.Aquaguard.Application.Features.Users.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Users.GetUser;

public class GetUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RequestGetUser> _validator;

    public GetUserUseCase(IUserRepository userRepository, IValidator<RequestGetUser> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<ResponseUser> ExecuteAsync(RequestGetUser request)
    {
        Validate(request, _validator);

        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
            throw new ErrorOnValidationException("Usuário não encontrado.");

        return new ResponseUser(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Role: user.Role.ToString());
    }

    public async Task<ResponseUsers> ListUsersAsync(Guid RequestedByUserId, Guid cityId , UserRole? RequestedByRole = null, int page = 1, int pageSize = 10)
    {
        List<ResponseUser> users = new List<ResponseUser>();
        IEnumerable<User>? usersOnDb = null;
        int total = 0;

        if (cityId == default && RequestedByRole == UserRole.Admin)
        {
            (usersOnDb, total) = await _userRepository.ListUsersAsync(page, pageSize);

        }
        else if (cityId != Guid.Empty && RequestedByRole == UserRole.Manager)
        {
            (usersOnDb, total) = await _userRepository.ListUsersPerCityAsync(page, pageSize, cityId);
        }
        else
        {
            throw new Exception("Falha ao buscar usuários");
        }

        foreach (var user in usersOnDb)
        {
            users.Add(new ResponseUser(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Role: nameof(user.Role)
            ));
        }

        return new ResponseUsers(
            amount: users.Count,
            total: total,
            page: page,
            totalPages: (int)Math.Ceiling((double)total / pageSize),
            users: users
            );
    }

    private static void Validate(RequestGetUser request, IValidator<RequestGetUser> validator)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
