namespace Application.Commands.CreatePayment;

using Contracts.Payments;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record CreatePaymentCommand(
    Guid BookingId,
    decimal Amount,
    Currency Currency,
    string Description
) : ICommand<PaymentResponseModel>;