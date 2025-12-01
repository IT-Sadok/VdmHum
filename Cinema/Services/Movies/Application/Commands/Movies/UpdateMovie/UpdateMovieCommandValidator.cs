namespace Application.Commands.Movies.UpdateMovie;

using FluentValidation;

public sealed class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.DurationMinutes)
            .GreaterThan(0)
            .When(c => c.DurationMinutes.HasValue);

        RuleFor(c => c.PosterUrl)
            .Must(url => Uri.IsWellFormedUriString(url!, UriKind.Absolute))
            .When(c => !string.IsNullOrWhiteSpace(c.PosterUrl))
            .WithMessage("Poster URL must be a valid absolute URL.");

        RuleFor(c => c.ReleaseDate)
            .NotEqual(default(DateOnly))
            .When(c => c.ReleaseDate.HasValue);
    }
}