namespace Application.Queries.GetShowtimes;

using FluentValidation;

public sealed class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
{
    public GetShowtimesQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(q => q)
            .Must(q =>
                !q.PagedFilter.ModelFilter.DateFromUtc.HasValue ||
                !q.PagedFilter.ModelFilter.DateToUtc.HasValue ||
                q.PagedFilter.ModelFilter.DateFromUtc <= q.PagedFilter.ModelFilter.DateToUtc)
            .WithMessage("DateFromUtc cannot be greater than DateToUtc.");

        RuleFor(q => q.PagedFilter.ModelFilter.DateFromUtc)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(q => q.PagedFilter.ModelFilter.DateFromUtc.HasValue)
            .WithMessage("DateFromUtc must be in UTC (DateTimeKind.Utc).");

        RuleFor(q => q.PagedFilter.ModelFilter.DateToUtc)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(q => q.PagedFilter.ModelFilter.DateToUtc.HasValue)
            .WithMessage("DateToUtc must be in UTC (DateTimeKind.Utc).");
    }
}