var builder = WebApplication.CreateBuilder(args);

//Add services

var app = builder.Build();

//Config http request pipeline

app.Run();
