namespace Application.Queries.GetCinemas;

using FluentValidation;

public sealed class GetCinemasQueryValidator : AbstractValidator<GetCinemasQuery>
{
    public GetCinemasQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);
    }
}