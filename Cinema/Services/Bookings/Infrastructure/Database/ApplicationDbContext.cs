namespace Infrastructure.Database;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Booking> Bookings => this.Set<Booking>();

    public DbSet<BookingSeat> BookingSeats => this.Set<BookingSeat>();

    public DbSet<Ticket> Tickets => this.Set<Ticket>();

    public DbSet<BookingRefund> Refunds => this.Set<BookingRefund>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}