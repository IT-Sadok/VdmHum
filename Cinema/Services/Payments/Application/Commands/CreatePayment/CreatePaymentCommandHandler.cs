namespace Application.Commands.CreatePayment;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Payments;
using Domain.Entities;
using Domain.ValueObjects;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CreatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderClient paymentProviderClient,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePaymentCommand, PaymentResponseModel>
{
    public async Task<Result<PaymentResponseModel>> HandleAsync(
        CreatePaymentCommand command,
        CancellationToken ct)
    {
        if (command.BookingId == Guid.Empty)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.InvalidId);
        }

        if (command.Amount <= 0)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.InvalidAmount);
        }

        var money = Money.From(command.Amount, command.Currency);

        var session = await paymentProviderClient.CreatePaymentSessionAsync(
            bookingId: command.BookingId,
            amount: money.Amount,
            currency: money.Currency,
            description: command.Description,
            ct: ct);

        var payment = Payment.Create(
            bookingId: command.BookingId,
            amount: money,
            provider: command.Provider,
            providerPaymentId: session.ProviderPaymentId,
            checkoutUrl: session.CheckoutUrl);

        paymentRepository.Add(payment);
        await unitOfWork.SaveChangesAsync(ct);

        return payment.ToResponse();
    }
}