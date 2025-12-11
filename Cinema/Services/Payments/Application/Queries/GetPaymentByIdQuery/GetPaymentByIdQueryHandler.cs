namespace Application.Queries.GetPaymentByIdQuery;

using Abstractions.Repositories;
using Contracts.Payments;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetPaymentByIdQueryHandler(
    IPaymentRepository paymentRepository)
    : IQueryHandler<GetPaymentByIdQuery, PaymentResponseModel>
{
    public async Task<Result<PaymentResponseModel>> HandleAsync(
        GetPaymentByIdQuery query,
        CancellationToken ct)
    {
        var payment = await paymentRepository.GetByIdAsync(query.PaymentId, asNoTracking: true, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentResponseModel>(CommonErrors.NotFound);
        }

        return payment.ToResponse();
    }
}