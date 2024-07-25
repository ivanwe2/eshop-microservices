using Basket.API.Data;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCarter()
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
        config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    })
    .AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
        options.Schema.For<ShoppingCart>().Identity(x=>x.UserName);
    }).ApplyAllDatabaseChangesOnStartup().UseLightweightSessions();


builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//Best Practice to decorate
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

//Second way
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepository!, provider.GetRequiredService<IDistributedCache>());
//});

//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.InitializeMartenWith<CatalogInitialData>();
//}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//builder.Services.AddHealthChecks()
//                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => {});

app.Run();
