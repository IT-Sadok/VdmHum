namespace Domain.Entities;

using Enums;

public sealed class MovieGenre
{
    private MovieGenre(Guid movieId, Genres genre)
    {
        this.MovieId = movieId;
        this.Genre = genre;
    }

    public Guid MovieId { get; private set; }

    public Movie Movie { get; private set; } = null!;

    public Genres Genre { get; private set; }

    internal static MovieGenre Create(Movie movie, Genres genre)
    {
        ArgumentNullException.ThrowIfNull(movie);

        return new MovieGenre(movie.Id, genre)
        {
            Movie = movie,
        };
    }
}