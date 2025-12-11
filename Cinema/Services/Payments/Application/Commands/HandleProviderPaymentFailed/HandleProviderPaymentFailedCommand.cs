namespace Application.Commands.HandleProviderPaymentFailed;

using Shared.Contracts.Abstractions;

public sealed record HandleProviderPaymentFailedCommand(
    string ProviderPaymentId,
    string FailureCode,
    string FailureMessage,
    DateTime FailedAtUtc
) : ICommand;