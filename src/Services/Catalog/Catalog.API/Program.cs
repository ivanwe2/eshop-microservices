using BuildingBlocks.Behaviours;

var builder = WebApplication.CreateBuilder(args);

//Add services
builder.Services
    .AddCarter()
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    })
    .AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();

var app = builder.Build();

//Configure HTTPS pipeline
app.MapCarter();

app.Run();
