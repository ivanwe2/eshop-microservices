using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add services
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


var app = builder.Build();

//Config http request pipeline

app.UseApiServices();

app.Run();
