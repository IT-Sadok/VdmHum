namespace Infrastructure.BackgroundServices;

using Application.Commands.ExpireReservation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Contracts.Abstractions;

public sealed class ExpireReservationsBackgroundService(IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.ExecuteCommandAsync(new ExpireReservationCommand(), ct);

            // TODO: Move magic number to some config
            await Task.Delay(TimeSpan.FromSeconds(15), ct);
        }
    }
}