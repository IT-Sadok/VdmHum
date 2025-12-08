namespace Application.Queries.GetUserBookings;

using FluentValidation;

public sealed class GetUserBookingsQueryValidator : AbstractValidator<GetUserBookingsQuery>
{
    public GetUserBookingsQueryValidator()
    {
        RuleFor(q => q.PagedFilter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PagedFilter.PageSize)
            .InclusiveBetween(1, 100);
    }
}