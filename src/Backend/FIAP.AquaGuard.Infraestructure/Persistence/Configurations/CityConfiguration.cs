using FIAP.AquaGuard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.AquaGuard.Infrastructure.Persistence.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Zipcode).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
        builder.Property(x => x.State).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Latitude).HasPrecision(10, 6);
        builder.Property(x => x.Longitude).HasPrecision(10, 6);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.Zipcode).IsUnique();

        builder.HasOne(x => x.ResponsibleUser)
            .WithMany(x => x.ResponsibleCities)
            .HasForeignKey(x => x.ResponsibleUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
