using Basket.API.Data;

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
        //options.CreateDatabasesForTenants(c => c.);
    }).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.InitializeMartenWith<CatalogInitialData>();
//}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//builder.Services.AddHealthChecks()
//                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

app.MapCarter();


app.Run();
