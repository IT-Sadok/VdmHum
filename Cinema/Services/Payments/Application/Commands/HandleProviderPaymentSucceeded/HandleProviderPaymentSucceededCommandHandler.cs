namespace Application.Commands.HandleProviderPaymentSucceeded;

using Abstractions.Repositories;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class HandleProviderPaymentSucceededCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<HandleProviderPaymentSucceededCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderPaymentSucceededCommand command,
        CancellationToken ct)
    {
        var payment = await paymentRepository
            .GetByProviderPaymentIdAsync(command.ProviderPaymentId, asNoTracking: false, ct);

        if (payment is null)
        {
            return Result.Failure(CommonErrors.NotFound);
        }

        payment.MarkSucceeded(command.SucceededAtUtc);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}