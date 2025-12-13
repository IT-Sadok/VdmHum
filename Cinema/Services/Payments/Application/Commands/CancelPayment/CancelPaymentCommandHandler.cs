namespace Application.Commands.CancelPayment;

using System.Net;
using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.PaymentProvider;
using Contracts.Payments;
using Domain.Enums;
using Errors;
using Exceptions;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CancelPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderClient paymentProviderClient,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelPaymentCommand, PaymentResponseModel>
{
    public async Task<Result<PaymentResponseModel>> HandleAsync(
        CancelPaymentCommand command,
        CancellationToken ct)
    {
        var payment = await paymentRepository.GetByIdAsync(command.PaymentId, asNoTracking: false, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.NotFound);
        }

        if (payment.Status != PaymentStatus.Pending)
        {
            return Result.Failure<PaymentResponseModel>(PaymentErrors.AlreadyProcessed);
        }

        var request = new CancelPaymentSessionRequest(payment.ProviderPaymentId);

        try
        {
            await paymentProviderClient.CancelPaymentSessionAsync(request, ct);
        }
        catch (PaymentProviderException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<PaymentResponseModel>(PaymentProviderErrors.BadRequest);
        }
        catch (PaymentProviderException)
        {
            return Result.Failure<PaymentResponseModel>(PaymentProviderErrors.ServerError);
        }

        payment.Cancel(DateTime.UtcNow);

        await unitOfWork.SaveChangesAsync(ct);

        return payment.ToResponse(includeRefunds: false);
    }
}