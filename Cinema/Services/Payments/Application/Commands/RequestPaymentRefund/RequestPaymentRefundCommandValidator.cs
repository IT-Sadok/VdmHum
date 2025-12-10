namespace Application.Commands.RequestPaymentRefund;

using FluentValidation;

public sealed class RequestPaymentRefundCommandValidator
    : AbstractValidator<RequestPaymentRefundCommand>
{
    public RequestPaymentRefundCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Currency must be a valid value.");

        RuleFor(x => x.Reason)
            .MaximumLength(1000);

        RuleFor(x => x.BookingRefundId)
            .NotEmpty()
            .WithMessage("BookingId is required.");
    }
}