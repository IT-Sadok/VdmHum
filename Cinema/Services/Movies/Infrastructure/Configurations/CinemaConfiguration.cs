namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Latitude);
        builder.Property(c => c.Longitude);

        builder.HasIndex(c => new { c.Name, c.City })
            .IsUnique();

        builder.HasMany(c => c.Halls)
            .WithOne()
            .HasForeignKey(h => h.CinemaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Halls)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}