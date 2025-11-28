namespace Domain.Entities;

using Enums;

public sealed class Movie
{
    private readonly HashSet<Genres> _genres;

    private Movie(
        Guid id,
        string title,
        string? description,
        HashSet<Genres> genres,
        int? durationMinutes,
        AgeRating? ageRating,
        Status status,
        DateOnly? releaseDate,
        string? posterUrl)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this._genres = genres;
        this.DurationMinutes = durationMinutes;
        this.AgeRating = ageRating;
        this.Status = status;
        this.ReleaseDate = releaseDate;
        this.PosterUrl = posterUrl;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public IReadOnlyCollection<Genres> Genres => this._genres;

    public int? DurationMinutes { get; private set; }

    public AgeRating? AgeRating { get; private set; }

    public Status Status { get; private set; }

    public DateOnly? ReleaseDate { get; private set; }

    public string? PosterUrl { get; private set; }

    public static Movie Create(
        string title,
        Status status,
        string? description = null,
        IEnumerable<Genres>? genres = null,
        int? durationMinutes = null,
        AgeRating? ageRating = null,
        DateOnly? releaseDate = null,
        string? posterUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        if (durationMinutes <= 0)
        {
            throw new ArgumentException(
                "Duration must be a positive number of minutes.",
                nameof(durationMinutes));
        }

        if (!string.IsNullOrWhiteSpace(posterUrl) &&
            !Uri.IsWellFormedUriString(posterUrl, UriKind.Absolute))
        {
            throw new ArgumentException(
                "Poster URL must be a valid absolute URL.",
                nameof(posterUrl));
        }

        var genresSet = genres is null
            ? new HashSet<Genres>()
            : new HashSet<Genres>(genres);

        var movie = new Movie(
            id: Guid.CreateVersion7(),
            title: title.Trim(),
            description: string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            genres: genresSet,
            durationMinutes: durationMinutes,
            ageRating: ageRating,
            status: status,
            releaseDate: releaseDate,
            posterUrl: string.IsNullOrWhiteSpace(posterUrl) ? null : posterUrl.Trim());

        return movie;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        this.Title = title.Trim();
    }

    public void UpdateDescription(string? description) =>
        this.Description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();

    public void UpdateDuration(int? durationMinutes)
    {
        if (durationMinutes <= 0)
        {
            throw new ArgumentException(
                "Duration must be a positive number of minutes.",
                nameof(durationMinutes));
        }

        this.DurationMinutes = durationMinutes;
    }

    public void UpdatePosterUrl(string? posterUrl)
    {
        if (!string.IsNullOrWhiteSpace(posterUrl) &&
            !Uri.IsWellFormedUriString(posterUrl, UriKind.Absolute))
        {
            throw new ArgumentException(
                "Poster URL must be a valid absolute URL.",
                nameof(posterUrl));
        }

        this.PosterUrl = string.IsNullOrWhiteSpace(posterUrl)
            ? null
            : posterUrl.Trim();
    }

    public void UpdateAgeRating(AgeRating ageRating) => this.AgeRating = ageRating;

    public void UpdateReleaseDate(DateOnly releaseDate)
    {
        if (releaseDate == default)
        {
            throw new ArgumentException("Release date is required.", nameof(releaseDate));
        }

        this.ReleaseDate = releaseDate;
    }

    public void SetGenres(IEnumerable<Genres> genres)
    {
        ArgumentNullException.ThrowIfNull(genres);

        this._genres.Clear();

        foreach (var genre in genres)
        {
            this._genres.Add(genre);
        }
    }

    public void AddGenre(Genres genre) => this._genres.Add(genre);

    public void RemoveGenre(Genres genre) => this._genres.Remove(genre);

    public void ChangeStatus(Status newStatus)
    {
        if (this.Status == newStatus)
        {
            return;
        }

        this.Status = newStatus;
    }
}