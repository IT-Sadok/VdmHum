namespace Application.Commands.SystemCancelBooking;

using FluentValidation;

public sealed class SystemCancelBookingCommandValidator
    : AbstractValidator<SystemCancelBookingCommand>
{
    public SystemCancelBookingCommandValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();
    }
}