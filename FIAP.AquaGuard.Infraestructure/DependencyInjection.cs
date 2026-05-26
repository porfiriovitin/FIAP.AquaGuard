using Fiap.AquaGuard.Infrastructure.Persistence;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.AquaGuard.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var host = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
        var database = Environment.GetEnvironmentVariable("DATABASE_NAME");
        var user = Environment.GetEnvironmentVariable("DATABASE_USER");
        var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

        var connectionString =
            $"Host={host};Port={port};Database={database};Username={user};Password={password}";

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.MapEnum<UserRole>("user_role");
            npgsql.MapEnum<SensorType>("sensor_type");
            npgsql.MapEnum<SensorStatus>("sensor_status");
            npgsql.MapEnum<RiskLevel>("risk_level");
            npgsql.MapEnum<RiskSourceType>("risk_source_type");
        }));

        return services;
    }
}