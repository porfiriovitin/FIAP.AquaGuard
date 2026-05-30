using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Providers;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Domain.Services;
using FIAP.AquaGuard.Infrastructure.Options;
using FIAP.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Infrastructure.Persistence.Repositories;
using FIAP.AquaGuard.Infrastructure.Services;
using FIAP.AquaGuard.Infrastructure.Services.OpenMeteo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.AquaGuard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>() ?? throw new InvalidOperationException("Database configuration is missing.");

        var connectionString = databaseOptions.ToConnectionString();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MapEnum<UserRole>("user_role");
                npgsql.MapEnum<SensorType>("sensor_type");
                npgsql.MapEnum<SensorStatus>("sensor_status");
                npgsql.MapEnum<RiskLevel>("risk_level");
                npgsql.MapEnum<RiskSourceType>("risk_source_type");
            })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>(); 
        services.AddScoped<ICityRepository, CityRepository>(); 
        services.AddScoped<ISensorRepository, SensorRepository>(); 
        services.AddScoped<ISensorReadingRepository, SensorReadingRepository>(); 
        services.AddScoped<IRiskAnalysisRepository, RiskAnalysisRepository>(); 
        services.AddScoped<IRiskAnalysisSensorReadingRepository, RiskAnalysisSensorReadingRepository>(); 
        services.AddScoped<IRiskDataSourceRepository, RiskDataSourceRepository>(); 

        services.AddScoped<ITokenProvider, JwtService>(); services.AddScoped<IUnitOfWork, UnitOfWork>(); 
        services.AddScoped<IPasswordHasherProvider, BCryptService>();

        services.AddHttpClient<IOpenMeteoProvider, OpenMeteoService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
        })
        .AddStandardResilienceHandler();


        return services;
    }
}
