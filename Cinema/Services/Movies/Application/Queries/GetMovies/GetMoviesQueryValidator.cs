namespace Application.Queries.GetMovies;

using FluentValidation;

public sealed class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesQueryValidator()
    {
        this.RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        this.RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);

        this.RuleFor(q => q.MinDurationMinutes)
            .GreaterThan(0)
            .When(q => q.MinDurationMinutes.HasValue);

        this.RuleFor(q => q.MaxDurationMinutes)
            .GreaterThan(0)
            .When(q => q.MaxDurationMinutes.HasValue);

        this.RuleFor(q => q)
            .Must(q =>
                !q.MinDurationMinutes.HasValue ||
                !q.MaxDurationMinutes.HasValue ||
                q.MinDurationMinutes <= q.MaxDurationMinutes)
            .WithMessage("MinDurationMinutes cannot be greater than MaxDurationMinutes.");

        this.RuleFor(q => q)
            .Must(q =>
                !q.MinAgeRating.HasValue ||
                !q.MaxAgeRating.HasValue ||
                q.MinAgeRating.Value <= q.MaxAgeRating.Value)
            .WithMessage("MinAgeRating cannot be greater than MaxAgeRating.");
    }
}