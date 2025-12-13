namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.BookingId)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.Provider)
            .IsRequired();

        builder.Property(p => p.ProviderPaymentId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.CheckoutUrl)
            .HasMaxLength(1000);

        builder.Property(p => p.FailureCode)
            .HasMaxLength(100);

        builder.Property(p => p.FailureMessage)
            .HasMaxLength(1000);

        builder.Property(p => p.CreatedAtUtc)
            .IsRequired();

        builder.Property(p => p.UpdatedAtUtc);

        builder.Property(p => p.SucceededAtUtc);

        builder.Property(p => p.FailedAtUtc);

        builder.Property(p => p.CanceledAtUtc);

        builder.OwnsOne(
            p => p.Amount,
            moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                moneyBuilder.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .IsRequired();
            });

        builder
            .HasMany(p => p.Refunds)
            .WithOne()
            .HasForeignKey(p => p.PaymentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Refunds)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.BookingId);

        builder.HasIndex(p => p.ProviderPaymentId)
            .IsUnique();
    }
}