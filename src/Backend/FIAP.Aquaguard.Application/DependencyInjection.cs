using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.AquaGuard.Application.Features.Auth.Register;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.AquaGuard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserAccountUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<IValidator<RequestLogin>, RequestLoginValidator>();
        services.AddScoped<IValidator<RequestRegisterUser>, RegisterUserAccountValidator>();

        return services;
    }
}
