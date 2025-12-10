namespace Application.Abstractions.Repositories;

using Domain.Entities;

public interface IPaymentRefundRepository
{
    Task<PaymentRefund?> GetByProviderRefundIdAsync(
        string providerRefundId,
        bool asNoTracking,
        CancellationToken ct);
}