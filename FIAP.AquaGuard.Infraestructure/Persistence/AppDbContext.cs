using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fiap.AquaGuard.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<City> Cities => Set<City>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<RiskAnalysis> RiskAnalyses => Set<RiskAnalysis>();
    public DbSet<RiskAnalysisSensorReading> RiskAnalysisSensorReadings => Set<RiskAnalysisSensorReading>();
    public DbSet<RiskDataSource> RiskDataSources => Set<RiskDataSource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}