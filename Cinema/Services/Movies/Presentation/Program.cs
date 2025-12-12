using System.Reflection;
using Application;
using Infrastructure;
using Presentation;
using Presentation.Extensions;
using Presentation.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<MoviesGrpcService>();

app.MapEndpoints();

await app.RunAsync();