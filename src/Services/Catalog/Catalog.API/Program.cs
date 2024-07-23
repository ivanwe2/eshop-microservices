var builder = WebApplication.CreateBuilder(args);

//Add services
builder.Services.AddCarter()
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                });

var app = builder.Build();

//Configure HTTPS pipeline
app.MapCarter();

app.Run();
