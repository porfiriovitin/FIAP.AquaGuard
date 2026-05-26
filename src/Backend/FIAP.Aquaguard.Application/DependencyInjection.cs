using FIAP.AquaGuard.Application.Features.Auth.Register;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.AquaGuard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserAccountUseCase>();

        return services;
    }
}