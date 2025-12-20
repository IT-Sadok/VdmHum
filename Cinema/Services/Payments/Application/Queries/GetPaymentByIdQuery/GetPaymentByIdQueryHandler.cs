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

        var payment = await paymentRepository.GetByIdForUserAsync(query.PaymentId, userId, asNoTracking: true, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.NotFound);
        }

        return payment.ToResponse();
    }
}