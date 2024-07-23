var builder = WebApplication.CreateBuilder(args);

//Add services
builder.Services.AddCarter()
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                })
                .AddMarten(options =>
                {
                    options.Connection(builder.Configuration.GetConnectionString("Database")!);
                }).UseLightweightSessions();

var app = builder.Build();

//Configure HTTPS pipeline
app.MapCarter();

app.Run();
