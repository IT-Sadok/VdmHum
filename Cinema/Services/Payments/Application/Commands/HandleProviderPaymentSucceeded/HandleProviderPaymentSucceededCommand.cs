namespace Application.Commands.HandleProviderPaymentSucceeded;

using Shared.Contracts.Abstractions;

public sealed record HandleProviderPaymentSucceededCommand(
    string ProviderPaymentId,
    DateTime SucceededAtUtc
) : ICommand;