namespace Infrastructure;

using System.Security.Cryptography.X509Certificates;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Options;
using Database;
using Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;
using Movies.Grpc;
using Payments.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddDatabase(configuration)
            .AddGrpcClients(configuration)
            .AddRepositories()
            .AddServices()
            .AddBackgroundServices()
            .AddJsonSerializerOptions()
            .AddAuthOptions(configuration)
            .AddAuthenticationInternal()
            .AddAuthorizationInternal();

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("Db:Host");
        var port = configuration.GetValue<int>("Db:Port");
        var dbName = configuration.GetValue<string>("Db:Name");
        var user = configuration.GetValue<string>("Db:User");
        var pass = configuration.GetValue<string>("Db:Password");

        var connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass}";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static IServiceCollection AddAuthOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }

    private static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<Movies.MoviesClient>(options =>
        {
            options.Address = new Uri(configuration["Grpc:MoviesServiceUrl"]
                                      ?? throw new InvalidOperationException());
        });
        services.AddGrpcClient<Payments.PaymentsClient>(options =>
        {
            options.Address = new Uri(configuration["Grpc:PaymentsServiceUrl"]
                                      ?? throw new InvalidOperationException());
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMoviesClient, MoviesGrpcClient>();
        services.AddScoped<IPaymentsClient, PaymentsGrpcClient>();
        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<PaymentsEventsConsumer>();

        return services;
    }

    private static IServiceCollection AddJsonSerializerOptions(this IServiceCollection services)
    {
        services.AddSingleton<EventJsonOptions>();

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddSingleton<X509Certificate2>(sp =>
        {
            var jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;
            return X509CertificateLoader.LoadCertificateFromFile(jwtOptions.SigningCertificatePath);
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>, X509Certificate2>((options, jwtOptionsAccessor, cert) =>
            {
                var jwt = jwtOptionsAccessor.Value;

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new X509SecurityKey(cert),

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents();
            });

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy => { policy.RequireRole("Admin"); });

        return services;
    }
}