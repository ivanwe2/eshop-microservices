using Basket.API.Data;
using Basket.API.DependencyInjection;
using BuildingBlocks.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddDataServices(configuration)
    .AddGrpcServices(configuration)
    .AddMessageBroker(configuration)
    .AddCrossCuttingConcernsServices(configuration);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => {});

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
