namespace Application.Commands.HandleProviderRefundFailer;

using FluentValidation;

public sealed class HandleProviderRefundFailedCommandValidator
    : AbstractValidator<HandleProviderRefundFailedCommand>
{
    public HandleProviderRefundFailedCommandValidator()
    {
        RuleFor(x => x.ProviderRefundId)
            .NotEmpty()
            .WithMessage("ProviderRefundId is required.");

        RuleFor(x => x.FailureCode)
            .NotEmpty()
            .WithMessage("FailureCode is required.")
            .MaximumLength(100);

        RuleFor(x => x.FailureMessage)
            .NotEmpty()
            .WithMessage("FailureMessage is required.")
            .MaximumLength(1000);

        RuleFor(x => x.FailedAtUtc)
            .NotEqual(default(DateTime))
            .WithMessage("FailedAtUtc must be a valid date.");
    }
}