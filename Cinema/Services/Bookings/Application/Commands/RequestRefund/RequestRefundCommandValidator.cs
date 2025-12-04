namespace Application.Commands.RequestRefund;

using FluentValidation;

public sealed class RequestRefundCommandValidator : AbstractValidator<RequestRefundCommand>
{
    public RequestRefundCommandValidator()
    {
        RuleFor(c => c.BookingId)
            .NotEmpty();
    }
}