namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class PaymentRefundRepository(ApplicationDbContext dbContext) : IPaymentRefundRepository
{
    public async Task<PaymentRefund?> GetByProviderRefundIdAsync(
        string providerRefundId,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<PaymentRefund> query = dbContext.PaymentRefunds;

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(pR => pR.ProviderRefundId == providerRefundId, ct);
    }
}