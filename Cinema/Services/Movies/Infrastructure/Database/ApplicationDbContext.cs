namespace Infrastructure.Database;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Movie> Movies => this.Set<Movie>();

    public DbSet<MovieGenre> MovieGenres => this.Set<MovieGenre>();

    public DbSet<Cinema> Cinemas => this.Set<Cinema>();

    public DbSet<Hall> Halls => this.Set<Hall>();

    public DbSet<Showtime> Showtimes => this.Set<Showtime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}