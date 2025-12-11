namespace Application.Commands.HandleProviderPaymentSucceeded;

using FluentValidation;

public sealed class HandleProviderPaymentSucceededCommandValidator
    : AbstractValidator<HandleProviderPaymentSucceededCommand>
{
    public HandleProviderPaymentSucceededCommandValidator()
    {
        RuleFor(x => x.ProviderPaymentId)
            .NotEmpty()
            .WithMessage("ProviderPaymentId is required.");

        RuleFor(x => x.SucceededAtUtc)
            .NotEqual(default(DateTime))
            .WithMessage("SucceededAtUtc must be a valid date.");
    }
}