namespace Application.Commands.CreatePayment;

using Contracts.Payments;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record CreatePaymentCommand(
    Guid BookingId,
    decimal Amount,
    Currency Currency,
    PaymentProvider Provider,
    string Description
) : ICommand<PaymentResponseModel>;