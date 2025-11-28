namespace Application.Queries.GetShowtimes;

using FluentValidation;

public sealed class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
{
    public GetShowtimesQueryValidator()
    {
        this.RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        this.RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);

        this.RuleFor(q => q)
            .Must(q =>
                !q.DateFromUtc.HasValue ||
                !q.DateToUtc.HasValue ||
                q.DateFromUtc <= q.DateToUtc)
            .WithMessage("DateFromUtc cannot be greater than DateToUtc.");

        this.RuleFor(q => q.DateFromUtc)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(q => q.DateFromUtc.HasValue)
            .WithMessage("DateFromUtc must be in UTC (DateTimeKind.Utc).");

        this.RuleFor(q => q.DateToUtc)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(q => q.DateToUtc.HasValue)
            .WithMessage("DateToUtc must be in UTC (DateTimeKind.Utc).");
    }
}