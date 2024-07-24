
var builder = WebApplication.CreateBuilder(args);

//Add services
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
    }).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure HTTPS pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
