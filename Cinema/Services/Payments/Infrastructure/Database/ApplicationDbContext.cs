namespace Infrastructure.Database;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Payment> Payments => this.Set<Payment>();

    public DbSet<PaymentRefund> PaymentRefunds => this.Set<PaymentRefund>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}