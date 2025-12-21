namespace Application.Abstractions.Repositories;

using Domain.Entities;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    Task<Payment?> GetByIdForUserAsync(
        Guid paymentId,
        Guid userId,
        bool asNoTracking,
        CancellationToken ct);

    Task<Payment?> GetByProviderPaymentIdAsync(
        string providerPaymentId,
        bool asNoTracking,
        CancellationToken ct);

    void Add(Payment payment);
}