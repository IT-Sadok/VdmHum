namespace Application.Queries.GetPaymentByIdQuery;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Payments;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetPaymentByIdQueryHandler(
    IPaymentRepository paymentRepository,
    IUserContextService userContextService)
    : IQueryHandler<GetPaymentByIdQuery, PaymentResponseModel>
{
    public async Task<Result<PaymentResponseModel>> HandleAsync(
        GetPaymentByIdQuery query,
        CancellationToken ct)
    {
        var userId = userContextService.GetUserContext().UserId!.Value;

        var payment = await paymentRepository.GetByIdAsync(query.PaymentId, asNoTracking: true, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.NotFound);
        }

        if (payment.UserId != userId)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.Forbidden);
        }

        return payment.ToResponse();
    }
}