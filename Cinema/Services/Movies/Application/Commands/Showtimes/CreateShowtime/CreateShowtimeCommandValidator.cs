namespace Application.Commands.Showtimes.CreateShowtime;

using FluentValidation;

public sealed class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
{
    public CreateShowtimeCommandValidator()
    {
        this.RuleFor(c => c.MovieId)
            .NotEmpty();

        this.RuleFor(c => c.CinemaId)
            .NotEmpty();

        this.RuleFor(c => c.HallId)
            .NotEmpty();

        this.RuleFor(c => c.StartTimeUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("StartTimeUtc must be in UTC (DateTimeKind.Utc).");

        this.RuleFor(c => c.EndTimeUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("EndTimeUtc must be in UTC (DateTimeKind.Utc).");

        this.RuleFor(c => c)
            .Must(c => c.EndTimeUtc > c.StartTimeUtc)
            .WithMessage("End time must be greater than start time.");

        this.RuleFor(c => c.BasePrice)
            .GreaterThanOrEqualTo(0);

        this.RuleFor(c => c.Currency)
            .NotEmpty()
            .Length(3)
            .Must(s => s.All(char.IsLetter))
            .WithMessage("Currency must be a 3-letter ISO 4217 code.");
    }
}