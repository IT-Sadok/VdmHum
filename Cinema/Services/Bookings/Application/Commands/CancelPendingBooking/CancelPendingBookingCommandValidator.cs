namespace Application.Commands.CancelPendingBooking;

using FluentValidation;

public sealed class CancelPendingBookingCommandValidator : AbstractValidator<CancelPendingBookingCommand>
{
    public CancelPendingBookingCommandValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();
    }
}