namespace Application.Commands.Showtimes.CreateShowtime;

using FluentValidation;

public sealed class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
{
    public CreateShowtimeCommandValidator()
    {
        RuleFor(c => c.MovieId)
            .NotEmpty();

        RuleFor(c => c.CinemaId)
            .NotEmpty();

        RuleFor(c => c.HallId)
            .NotEmpty();

        RuleFor(c => c.StartTimeUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("StartTimeUtc must be in UTC (DateTimeKind.Utc).");

        RuleFor(c => c.EndTimeUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("EndTimeUtc must be in UTC (DateTimeKind.Utc).");

        RuleFor(c => c)
            .Must(c => c.EndTimeUtc > c.StartTimeUtc)
            .WithMessage("End time must be greater than start time.");

        RuleFor(c => c.BasePrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(c => c.Currency)
            .NotEmpty()
            .Length(3)
            .Must(s => s.All(char.IsLetter))
            .WithMessage("Currency must be a 3-letter ISO 4217 code.");
    }
}