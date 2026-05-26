using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infraestructure.Persistence.Configurations;

public class RiskDataSourceConfiguration : IEntityTypeConfiguration<RiskDataSource>
{
    public void Configure(EntityTypeBuilder<RiskDataSource> builder)
    {
        builder.ToTable("risk_data_sources");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceType).IsRequired();
        builder.Property(x => x.ProviderName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.RawUrl).HasMaxLength(1000);
        builder.Property(x => x.RetrievedAt).IsRequired();

        builder.HasOne(x => x.RiskAnalysis)
            .WithMany(x => x.RiskDataSources)
            .HasForeignKey(x => x.RiskAnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
