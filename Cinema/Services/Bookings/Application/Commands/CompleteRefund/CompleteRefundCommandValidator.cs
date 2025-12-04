namespace Application.Commands.CompleteRefund;

using FluentValidation;

public sealed class CompleteRefundCommandValidator
    : AbstractValidator<CompleteRefundCommand>
{
    public CompleteRefundCommandValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();
    }
}