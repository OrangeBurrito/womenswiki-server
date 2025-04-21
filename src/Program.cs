using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore;
using WomensWiki;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("WikiConnection")!, assembly);
builder.Services.AddApplication(assembly);
builder.Services.AddApi(builder.Configuration["AllowedOrigins"]);

var app = builder.Build();

if (app.Environment.IsProduction()) {
    app.UseCors("Prod");
} else {
    app.UseCors("Localhost");
}

app.MapGraphQL().WithOptions(new GraphQLServerOptions { Tool = { Enable = app.Environment.IsDevelopment() } });

app.Run();