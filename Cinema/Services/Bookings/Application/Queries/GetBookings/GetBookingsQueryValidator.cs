namespace Application.Queries.GetBookings;

using FluentValidation;

public sealed class GetBookingsQueryValidator : AbstractValidator<GetBookingsQuery>
{
    public GetBookingsQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);
    }
}