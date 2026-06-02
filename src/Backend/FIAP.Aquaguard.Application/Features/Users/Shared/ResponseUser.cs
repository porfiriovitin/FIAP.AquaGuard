namespace FIAP.Aquaguard.Application.Features.Users.Shared;

public record ResponseUser(Guid Id, string Name, string Email, string Role);

public record ResponseUsers(
    long amount,
    long total,
    long page,
    long totalPages,
    List<ResponseUser> users
    );
