namespace Application.Queries.GetHalls;

using FluentValidation;

public sealed class GetHallsQueryValidator : AbstractValidator<GetHallsQuery>
{
    public GetHallsQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);
    }
}