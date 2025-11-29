namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .ValueGeneratedNever();

        builder.Property(h => h.CinemaId)
            .IsRequired();

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.NumberOfSeats)
            .IsRequired();

        builder.HasIndex(h => new { h.CinemaId, h.Name })
            .IsUnique();
    }
}