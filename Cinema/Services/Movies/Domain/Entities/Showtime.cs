namespace Domain.Entities;

using Enums;

public sealed class Showtime
{
    private Showtime(
        Guid id,
        Guid movieId,
        Guid cinemaId,
        Guid hallId,
        DateTime startTimeUtc,
        DateTime endTimeUtc,
        decimal basePrice,
        string currency,
        ShowtimeStatus status,
        string? language,
        string? format,
        string? cancelReason)
    {
        this.Id = id;
        this.MovieId = movieId;
        this.CinemaId = cinemaId;
        this.HallId = hallId;
        this.StartTimeUtc = startTimeUtc;
        this.EndTimeUtc = endTimeUtc;
        this.BasePrice = basePrice;
        this.Currency = currency;
        this.Status = status;
        this.Language = language;
        this.Format = format;
        this.CancelReason = cancelReason;
    }

    public Guid Id { get; }

    public Guid MovieId { get; private set; }

    public Guid CinemaId { get; private set; }

    public Guid HallId { get; private set; }

    public DateTime StartTimeUtc { get; private set; }

    public DateTime EndTimeUtc { get; private set; }

    public decimal BasePrice { get; private set; }

    public string Currency { get; private set; }

    public ShowtimeStatus Status { get; private set; }

    public string? Language { get; private set; }

    public string? Format { get; private set; }

    public string? CancelReason { get; private set; }

    public static Showtime Create(
        Guid movieId,
        Guid cinemaId,
        Guid hallId,
        DateTime startTimeUtc,
        DateTime endTimeUtc,
        decimal basePrice,
        string currency,
        ShowtimeStatus status = ShowtimeStatus.Scheduled,
        string? language = null,
        string? format = null)
    {
        if (movieId == Guid.Empty)
        {
            throw new ArgumentException("MovieId is required.", nameof(movieId));
        }

        if (cinemaId == Guid.Empty)
        {
            throw new ArgumentException("CinemaId is required.", nameof(cinemaId));
        }

        if (hallId == Guid.Empty)
        {
            throw new ArgumentException("HallId is required.", nameof(hallId));
        }

        if (endTimeUtc <= startTimeUtc)
        {
            throw new ArgumentException("End time must be greater than start time.", nameof(endTimeUtc));
        }

        if (basePrice < 0)
        {
            throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency is required.", nameof(currency));
        }

        currency = currency.Trim().ToUpperInvariant();
        if (currency.Length != 3)
        {
            throw new ArgumentException("Currency must be a 3-letter ISO 4217 code.", nameof(currency));
        }

        var showtime = new Showtime(
            id: Guid.CreateVersion7(),
            movieId: movieId,
            cinemaId: cinemaId,
            hallId: hallId,
            startTimeUtc: startTimeUtc,
            endTimeUtc: endTimeUtc,
            basePrice: basePrice,
            currency: currency,
            status: status,
            language: string.IsNullOrWhiteSpace(language) ? null : language.Trim(),
            format: string.IsNullOrWhiteSpace(format) ? null : format.Trim(),
            cancelReason: null);

        return showtime;
    }

    public void Reschedule(DateTime newStartTimeUtc, DateTime newEndTimeUtc)
    {
        if (this.Status == ShowtimeStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot reschedule a cancelled showtime.");
        }

        if (newEndTimeUtc <= newStartTimeUtc)
        {
            throw new ArgumentException("End time must be greater than start time.", nameof(newEndTimeUtc));
        }

        this.StartTimeUtc = newStartTimeUtc;
        this.EndTimeUtc = newEndTimeUtc;
    }

    public void UpdateBasePrice(decimal newBasePrice)
    {
        if (this.Status == ShowtimeStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot change price for a cancelled showtime.");
        }

        if (newBasePrice < 0)
        {
            throw new ArgumentException("Base price cannot be negative.", nameof(newBasePrice));
        }

        this.BasePrice = newBasePrice;
    }

    public void UpdateLanguage(string? language)
    {
        this.Language = string.IsNullOrWhiteSpace(language)
            ? null
            : language.Trim();
    }

    public void UpdateFormat(string? format)
    {
        this.Format = string.IsNullOrWhiteSpace(format)
            ? null
            : format.Trim();
    }

    public void ChangeStatus(ShowtimeStatus newStatus)
    {
        if (this.Status == newStatus)
        {
            return;
        }

        if (this.Status == ShowtimeStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot change status of a cancelled showtime.");
        }

        if (newStatus == ShowtimeStatus.SoldOut && this.Status != ShowtimeStatus.Active)
        {
            throw new InvalidOperationException("Only an active showtime can be marked as sold out.");
        }

        this.Status = newStatus;
    }

    public void Cancel(string? reason = null)
    {
        if (this.Status == ShowtimeStatus.Cancelled)
        {
            return;
        }

        this.Status = ShowtimeStatus.Cancelled;
        this.CancelReason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
    }

    public void MarkSoldOut()
    {
        if (this.Status == ShowtimeStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot mark cancelled showtime as sold out.");
        }

        if (this.Status != ShowtimeStatus.Active && this.Status != ShowtimeStatus.Scheduled)
        {
            throw new InvalidOperationException("Only scheduled or active showtime can become sold out.");
        }

        this.Status = ShowtimeStatus.SoldOut;
        this.CancelReason = null;
    }
}