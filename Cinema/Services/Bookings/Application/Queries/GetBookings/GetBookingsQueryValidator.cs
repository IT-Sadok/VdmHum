namespace Application.Queries.GetBookings;

using FluentValidation;

public sealed class GetBookingsQueryValidator : AbstractValidator<GetBookingsQuery>
{
    public GetBookingsQueryValidator()
    {
        RuleFor(q => q.Filter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.Filter.PageSize)
            .InclusiveBetween(1, 100);
    }
}