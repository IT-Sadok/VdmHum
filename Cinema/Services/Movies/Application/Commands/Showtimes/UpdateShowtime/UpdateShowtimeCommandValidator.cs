namespace Application.Commands.Showtimes.UpdateShowtime;

using FluentValidation;

public sealed class UpdateShowtimeCommandValidator : AbstractValidator<UpdateShowtimeCommand>
{
    public UpdateShowtimeCommandValidator()
    {
        RuleFor(c => c.Id)
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

        RuleFor(c => c.CancelReason)
            .MaximumLength(500)
            .When(c => !string.IsNullOrWhiteSpace(c.CancelReason));
    }
}