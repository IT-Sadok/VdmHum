namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.BookingId)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.RequestedAtUtc)
            .IsRequired();

        builder.Property(r => r.ProcessedAtUtc);

        builder.Property(r => r.PaymentId)
            .HasMaxLength(200);

        builder.Property(r => r.FailureReason)
            .HasMaxLength(1000);

        builder.OwnsOne(
            r => r.Amount,
            money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .IsRequired();
            });
    }
}