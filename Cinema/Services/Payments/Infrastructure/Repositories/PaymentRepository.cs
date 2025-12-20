namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository(ApplicationDbContext dbContext) : IPaymentRepository
{
    public async Task<Payment?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Payment> query = dbContext
            .Payments
            .Include(p => p.Refunds);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Payment?> GetByIdForUserAsync(
        Guid paymentId,
        Guid userId,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Payment> query = dbContext
            .Payments
            .Include(p => p.Refunds);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(p => p.Id == paymentId && p.UserId == userId, ct);
    }

    public async Task<Payment?> GetByProviderPaymentIdAsync(
        string providerPaymentId,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Payment> query = dbContext
            .Payments
            .Include(p => p.Refunds);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(p => p.ProviderPaymentId == providerPaymentId, ct);
    }

    public void Add(Payment payment) =>
        dbContext.Payments.Add(payment);
}