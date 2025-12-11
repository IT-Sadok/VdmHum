namespace Application.Commands.CreatePayment;

using FluentValidation;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithMessage("BookingId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Currency must be a valid value.");

        RuleFor(x => x.Provider)
            .IsInEnum()
            .WithMessage("Provider must be a valid value.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500);
    }
}