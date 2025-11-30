namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(m => m.Title)
            .IsUnique();

        builder.Property(m => m.Description)
            .HasMaxLength(4000);

        builder.Property(m => m.DurationMinutes);

        builder.Property(m => m.AgeRating)
            .HasConversion<int?>();

        builder.Property(m => m.Status)
            .HasConversion<int>();

        var releaseDateConverter = new ValueConverter<DateOnly?, DateTime?>(
            d => d.HasValue
                ? d.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
                : null,
            d => d.HasValue
                ? DateOnly.FromDateTime(DateTime.SpecifyKind(d.Value, DateTimeKind.Utc))
                : null);

        builder.Property(m => m.ReleaseDate)
            .HasConversion(releaseDateConverter);

        builder.Property(m => m.PosterUrl)
            .HasMaxLength(2048);

        builder.HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}