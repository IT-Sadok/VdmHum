namespace Application.Commands.HandleProviderRefundSucceeded;

using Shared.Contracts.Abstractions;

public sealed record HandleProviderRefundSucceededCommand(
    string ProviderRefundId,
    DateTime SucceededAtUtc
) : ICommand;