namespace Application.Queries.GetMovies;

using FluentValidation;

public sealed class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(q => q.PagedFilter.ModelFilter.MinDurationMinutes)
            .GreaterThan(0)
            .When(q => q.PagedFilter.ModelFilter.MinDurationMinutes.HasValue);

        RuleFor(q => q.PagedFilter.ModelFilter.MaxDurationMinutes)
            .GreaterThan(0)
            .When(q => q.PagedFilter.ModelFilter.MaxDurationMinutes.HasValue);

        RuleFor(q => q)
            .Must(q =>
                !q.PagedFilter.ModelFilter.MinDurationMinutes.HasValue ||
                !q.PagedFilter.ModelFilter.MaxDurationMinutes.HasValue ||
                q.PagedFilter.ModelFilter.MinDurationMinutes <= q.PagedFilter.ModelFilter.MaxDurationMinutes)
            .WithMessage("MinDurationMinutes cannot be greater than MaxDurationMinutes.");

        RuleFor(q => q)
            .Must(q =>
                !q.PagedFilter.ModelFilter.MinAgeRating.HasValue ||
                !q.PagedFilter.ModelFilter.MaxAgeRating.HasValue ||
                q.PagedFilter.ModelFilter.MinAgeRating.Value <= q.PagedFilter.ModelFilter.MaxAgeRating.Value)
            .WithMessage("MinAgeRating cannot be greater than MaxAgeRating.");
    }
}