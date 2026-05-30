using FIAP.Aquaguard.Application.Features.Auth.Login;
using FIAP.Aquaguard.Application.Features.Cities.CreateCity;
using FIAP.Aquaguard.Application.Features.Cities.DeleteCity;
using FIAP.Aquaguard.Application.Features.Cities.GetCity;
using FIAP.Aquaguard.Application.Features.Cities.UpdateCity;
using FIAP.Aquaguard.Application.Features.Cities.UpdateCityPaidPlan;
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
        services.AddScoped<CreateCityUseCase>();
        services.AddScoped<GetCityUseCase>();
        services.AddScoped<UpdateCityUseCase>();
        services.AddScoped<DeleteCityUseCase>();
        services.AddScoped<UpdateCityPaidPlanUseCase>();
        services.AddScoped<IValidator<RequestLogin>, RequestLoginValidator>();
        services.AddScoped<IValidator<RequestRegisterUser>, RegisterUserAccountValidator>();
        services.AddScoped<IValidator<RequestGetUser>, RequestGetUserValidator>();
        services.AddScoped<IValidator<RequestUpdateUser>, RequestUpdateUserValidator>();
        services.AddScoped<IValidator<RequestDeleteUser>, RequestDeleteUserValidator>();
        services.AddScoped<IValidator<RequestCreateCity>, RequestCreateCityValidator>();
        services.AddScoped<IValidator<RequestGetCity>, RequestGetCityValidator>();
        services.AddScoped<IValidator<RequestUpdateCity>, RequestUpdateCityValidator>();
        services.AddScoped<IValidator<RequestDeleteCity>, RequestDeleteCityValidator>();
        services.AddScoped<IValidator<RequestUpdateCityPaidPlan>, RequestUpdateCityPaidPlanValidator>();

        return services;
    }
}
