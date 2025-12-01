namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.ToTable("MovieGenres");

        builder.HasKey(mg => new { mg.MovieId, mg.Genre });

        builder.Property(mg => mg.MovieId)
            .IsRequired();

        builder.Property(mg => mg.Genre)
            .IsRequired()
            .HasConversion<int>();
    }
}