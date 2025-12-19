namespace Application.Commands.CreatePayment;

using System.Net;
using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.PaymentProvider;
using Contracts.Payments;
using Domain.Entities;
using Domain.ValueObjects;
using Errors;
using Exceptions;
using Microsoft.Extensions.Options;
using Options;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CreatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderClient paymentProviderClient,
    IOptions<PaymentOptions> options,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePaymentCommand, PaymentResponseModel>
{
    public async Task<Result<PaymentResponseModel>> HandleAsync(
        CreatePaymentCommand command,
        CancellationToken ct)
    {
        var money = Money.From(command.Amount, command.Currency);

        var request = new CreatePaymentSessionRequest(
            command.BookingId,
            money.Amount,
            money.Currency,
            command.Description);

        CreatePaymentSessionResult session;

        try
        {
            session = await paymentProviderClient.CreatePaymentSessionAsync(request, ct);
        }
        catch (PaymentProviderException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<PaymentResponseModel>(PaymentProviderErrors.BadRequest);
        }
        catch (PaymentProviderException)
        {
            return Result.Failure<PaymentResponseModel>(PaymentProviderErrors.ServerError);
        }

        var provider = options.Value.DefaultProvider;

        var payment = Payment.Create(
            bookingId: command.BookingId,
            amount: money,
            provider: provider,
            providerPaymentId: session.ProviderPaymentId,
            checkoutUrl: session.CheckoutUrl);

        paymentRepository.Add(payment);
        await unitOfWork.SaveChangesAsync(ct);

        return payment.ToResponse();
    }
}