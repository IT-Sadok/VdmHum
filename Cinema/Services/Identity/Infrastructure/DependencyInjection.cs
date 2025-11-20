namespace Infrastructure;

using Application.Abstractions.Providers;
using Identity;
using Providers;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services
            .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}