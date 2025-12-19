namespace Application.Commands.ProcessBookingPayment;

using FluentValidation;

public sealed class ProcessBookingPaymentCommandValidator
    : AbstractValidator<ProcessBookingPaymentCommand>
{
    public ProcessBookingPaymentCommandValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();

        RuleFor(c => c.PaymentId)
            .NotEmpty();
    }
}