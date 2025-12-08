namespace Application;

using Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Behaviours;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(AuthenticationCommandHandlerDecorator<,>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(AuthenticationQueryHandlerDecorator<,>));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }
}