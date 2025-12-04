namespace Application.Commands.FailRefund;

using FluentValidation;

public sealed class FailRefundValidator
    : AbstractValidator<FailRefundCommand>
{
    public FailRefundValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();

        RuleFor(c => c.FailureReason)
            .NotEmpty()
            .MaximumLength(1000);
    }
}