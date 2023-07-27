var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWikiServices(builder);

var app = builder.Build();

app.UseCors();

app.AddApiEndpoints();

app.Run();
