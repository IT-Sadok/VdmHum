namespace Application.Commands.HandleProviderPaymentFailed;

using Abstractions.Repositories;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class HandleProviderPaymentFailedCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<HandleProviderPaymentFailedCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderPaymentFailedCommand command,
        CancellationToken ct)
    {
        var payment = await paymentRepository
            .GetByProviderPaymentIdAsync(command.ProviderPaymentId, asNoTracking: false, ct);

        if (payment is null)
        {
            return Result.Failure(CommonErrors.NotFound);
        }

        payment.MarkFailed(
            failureCode: command.FailureCode,
            failureMessage: command.FailureMessage,
            failedAtUtc: command.FailedAtUtc);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}