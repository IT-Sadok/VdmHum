namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
{
    public void Configure(EntityTypeBuilder<Showtime> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.MovieId)
            .IsRequired();

        builder.Property(s => s.CinemaId)
            .IsRequired();

        builder.Property(s => s.HallId)
            .IsRequired();

        builder.Property(s => s.StartTimeUtc)
            .IsRequired();

        builder.Property(s => s.EndTimeUtc)
            .IsRequired();

        builder.Property(s => s.BasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.Language)
            .HasMaxLength(10);

        builder.Property(s => s.Format)
            .HasMaxLength(20);

        builder.Property(s => s.CancelReason)
            .HasMaxLength(200);

        builder.HasIndex(s => new { s.HallId, s.StartTimeUtc, s.EndTimeUtc });
        builder.HasIndex(s => s.MovieId);
        builder.HasIndex(s => s.CinemaId);

        builder.HasOne<Movie>()
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Cinema>()
            .WithMany()
            .HasForeignKey(s => s.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Hall>()
            .WithMany()
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}