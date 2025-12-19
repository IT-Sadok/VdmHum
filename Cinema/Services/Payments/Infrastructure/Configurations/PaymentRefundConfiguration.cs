namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class PaymentRefundConfiguration : IEntityTypeConfiguration<PaymentRefund>
{
    public void Configure(EntityTypeBuilder<PaymentRefund> builder)
    {
        builder.ToTable("PaymentRefunds");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.PaymentId)
            .IsRequired();

        builder.Property(r => r.BookingRefundId)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.ProviderRefundId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(r => r.Reason)
            .HasMaxLength(1000);

        builder.Property(r => r.FailureCode)
            .HasMaxLength(100);

        builder.Property(r => r.FailureMessage)
            .HasMaxLength(1000);

        builder.Property(r => r.RequestedAtUtc)
            .IsRequired();

        builder.Property(r => r.SucceededAtUtc);

        builder.Property(r => r.FailedAtUtc);

        builder.OwnsOne(r => r.Amount, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .IsRequired();
        });

        builder.HasIndex(r => r.PaymentId);

        builder.HasIndex(r => r.BookingRefundId);

        builder.HasIndex(r => r.ProviderRefundId)
            .IsUnique();
    }
}