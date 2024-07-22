var builder = WebApplication.CreateBuilder(args);

//Add services

var app = builder.Build();

//Configure HTTPS pipeline

app.Run();
