namespace Application.Queries.GetUserBookings;

using FluentValidation;

public sealed class GetUserBookingsQueryValidator : AbstractValidator<GetUserBookingsQuery>
{
    public GetUserBookingsQueryValidator()
    {
        RuleFor(q => q.Filter.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.Filter.PageSize)
            .InclusiveBetween(1, 100);
    }
}