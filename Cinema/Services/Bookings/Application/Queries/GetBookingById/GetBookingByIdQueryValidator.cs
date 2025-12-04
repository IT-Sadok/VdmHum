namespace Application.Queries.GetBookingById;

using FluentValidation;

public sealed class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdQueryValidator()
    {
        RuleFor(q => q.BookingId)
            .NotEmpty();
    }
}