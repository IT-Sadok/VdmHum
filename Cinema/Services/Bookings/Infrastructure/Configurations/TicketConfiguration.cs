namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.BookingId)
            .IsRequired();

        builder.Property(t => t.SeatNumber)
            .IsRequired();

        builder.Property(t => t.TicketNumber)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.QrCode)
            .HasMaxLength(500);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.IssuedAtUtc)
            .IsRequired();

        builder.Property(t => t.CancelledAtUtc);
    }
}