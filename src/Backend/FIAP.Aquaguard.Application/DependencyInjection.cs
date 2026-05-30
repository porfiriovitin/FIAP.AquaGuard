using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.AquaGuard.Application.Features.Auth.Register;
using FIAP.Aquaguard.Application.Features.Users.DeleteUser;
using FIAP.Aquaguard.Application.Features.Users.GetUser;
using FIAP.Aquaguard.Application.Features.Users.UpdateUser;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.AquaGuard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserAccountUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<GetUserUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<IValidator<RequestLogin>, RequestLoginValidator>();
        services.AddScoped<IValidator<RequestRegisterUser>, RegisterUserAccountValidator>();
        services.AddScoped<IValidator<RequestGetUser>, RequestGetUserValidator>();
        services.AddScoped<IValidator<RequestUpdateUser>, RequestUpdateUserValidator>();
        services.AddScoped<IValidator<RequestDeleteUser>, RequestDeleteUserValidator>();

        return services;
    }
}
