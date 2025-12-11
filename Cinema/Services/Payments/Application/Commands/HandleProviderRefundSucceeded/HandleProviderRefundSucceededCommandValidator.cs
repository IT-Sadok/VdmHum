namespace Application.Commands.HandleProviderRefundSucceeded;

using FluentValidation;

public sealed class HandleProviderRefundSucceededCommandValidator
    : AbstractValidator<HandleProviderRefundSucceededCommand>
{
    public HandleProviderRefundSucceededCommandValidator()
    {
        RuleFor(x => x.ProviderRefundId)
            .NotEmpty()
            .WithMessage("ProviderRefundId is required.");

        RuleFor(x => x.SucceededAtUtc)
            .NotEqual(default(DateTime))
            .WithMessage("SucceededAtUtc must be a valid date.");
    }
}