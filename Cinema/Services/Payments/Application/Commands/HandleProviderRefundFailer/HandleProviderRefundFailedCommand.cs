namespace Application.Commands.HandleProviderRefundFailer;

using Shared.Contracts.Abstractions;

public sealed record HandleProviderRefundFailedCommand(
    string ProviderRefundId,
    string FailureCode,
    string FailureMessage,
    DateTime FailedAtUtc
) : ICommand;