namespace Infrastructure.Configurations;

using Domain.Entities;
using Domain.Enums;
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

        var genresConverter = new ValueConverter<HashSet<Genres>, string>(
            v => string.Join(',', v.Select(g => ((int)g).ToString())),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => (Genres)int.Parse(s))
                .ToHashSet());

        builder.Property<HashSet<Genres>>("_genres")
            .HasConversion(genresConverter);

        builder.Navigation(m => m.Genres)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

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
    }
}