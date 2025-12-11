namespace Application.Commands.CancelPayment;

using Contracts.Payments;
using Shared.Contracts.Abstractions;

public record CancelPaymentCommand(Guid PaymentId) : ICommand<PaymentResponseModel>;