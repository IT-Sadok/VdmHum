namespace Application.Queries.GetCinemas;

using FluentValidation;

public sealed class GetCinemasQueryValidator : AbstractValidator<GetCinemasQuery>
{
    public GetCinemasQueryValidator()
    {
        this.RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        this.RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);
    }
}