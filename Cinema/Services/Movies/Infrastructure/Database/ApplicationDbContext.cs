namespace Infrastructure.Database;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Movie> Movies => this.Set<Movie>();

    public DbSet<Cinema> Cinemas => this.Set<Cinema>();

    public DbSet<Hall> Halls => this.Set<Hall>();

    public DbSet<Showtime> Showtimes => this.Set<Showtime>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}