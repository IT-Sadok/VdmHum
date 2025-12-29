namespace Infrastructure.BackgroundServices;

using Application.Commands.ExpireReservation;
using Application.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Contracts.Abstractions;

public sealed class ExpireReservationsBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<ExpireReservationsOptions> expireReservationsOptions)
    : BackgroundService
{
    private readonly ExpireReservationsOptions _options = expireReservationsOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.ExecuteCommandAsync(new ExpireReservationCommand(), ct);

            await Task.Delay(TimeSpan.FromSeconds(this._options.DelaySeconds), ct);
        }
    }
}