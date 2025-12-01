namespace Application.Queries.GetMovies;

using FluentValidation;

public sealed class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(q => q.MinDurationMinutes)
            .GreaterThan(0)
            .When(q => q.MinDurationMinutes.HasValue);

        RuleFor(q => q.MaxDurationMinutes)
            .GreaterThan(0)
            .When(q => q.MaxDurationMinutes.HasValue);

        RuleFor(q => q)
            .Must(q =>
                !q.MinDurationMinutes.HasValue ||
                !q.MaxDurationMinutes.HasValue ||
                q.MinDurationMinutes <= q.MaxDurationMinutes)
            .WithMessage("MinDurationMinutes cannot be greater than MaxDurationMinutes.");

        RuleFor(q => q)
            .Must(q =>
                !q.MinAgeRating.HasValue ||
                !q.MaxAgeRating.HasValue ||
                q.MinAgeRating.Value <= q.MaxAgeRating.Value)
            .WithMessage("MinAgeRating cannot be greater than MaxAgeRating.");
    }
}