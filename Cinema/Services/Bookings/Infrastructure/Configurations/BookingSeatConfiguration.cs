namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BookingSeatConfiguration : IEntityTypeConfiguration<BookingSeat>
{
    public void Configure(EntityTypeBuilder<BookingSeat> builder)
    {
        builder.ToTable("BookingSeats");

        builder.HasKey(bs => bs.Id);

        builder.Property(bs => bs.BookingId)
            .IsRequired();

        builder.Property(bs => bs.ShowtimeId)
            .IsRequired();

        builder.Property(bs => bs.SeatNumber)
            .IsRequired();

        builder.HasIndex(bs => new { bs.ShowtimeId, bs.SeatNumber })
            .IsUnique();
    }
}