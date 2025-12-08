namespace Application.Queries.GetHalls;

using FluentValidation;

public sealed class GetHallsQueryValidator : AbstractValidator<GetHallsQuery>
{
    public GetHallsQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);
    }
}