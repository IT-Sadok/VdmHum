namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired();

        builder.Property(b => b.CreatedAtUtc)
            .IsRequired();

        builder.Property(b => b.ReservationExpiresAtUtc)
            .IsRequired();

        builder.Property(b => b.UpdatedAtUtc);

        builder.Property(b => b.PaymentId);

        builder.Property(b => b.CancellationReason);

        builder.OwnsOne(
            b => b.TotalPrice,
            money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("TotalPrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .IsRequired();
            });

        builder.OwnsOne(
            b => b.Showtime,
            ss =>
            {
                ss.Property(x => x.ShowtimeId)
                    .HasColumnName("ShowtimeId")
                    .IsRequired();

                ss.Property(x => x.MovieId)
                    .HasColumnName("MovieId")
                    .IsRequired();

                ss.Property(x => x.CinemaId)
                    .HasColumnName("CinemaId")
                    .IsRequired();

                ss.Property(x => x.HallId)
                    .HasColumnName("HallId")
                    .IsRequired();

                ss.Property(x => x.MovieTitle)
                    .HasColumnName("MovieTitle")
                    .HasMaxLength(200)
                    .IsRequired();

                ss.Property(x => x.CinemaName)
                    .HasColumnName("CinemaName")
                    .HasMaxLength(200)
                    .IsRequired();

                ss.Property(x => x.HallName)
                    .HasColumnName("HallName")
                    .HasMaxLength(100)
                    .IsRequired();

                ss.Property(x => x.StartTimeUtc)
                    .HasColumnName("ShowtimeStartTimeUtc")
                    .IsRequired();
            });

        builder.HasMany(b => b.Seats)
            .WithOne()
            .HasForeignKey(t => t.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.Seats)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(b => b.Tickets)
            .WithOne()
            .HasForeignKey(t => t.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.Tickets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(b => b.Refunds)
            .WithOne()
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.Refunds)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}