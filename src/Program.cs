using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore;
using WomensWiki;
using WomensWiki.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("WikiConnection")!, assembly);
builder.Services.AddApplication(assembly);
builder.Services.AddApi(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

app.UseMiddleware<APIKeyMiddleware>();

if (app.Environment.IsProduction()) {
    app.UseCors("Prod");
} else {
    app.UseCors("Localhost");
}

if (!string.IsNullOrEmpty(builder.Configuration["Auth:BaseUrl"]))
    app.UseAuthentication();

app.UseRateLimiter();

app.MapGraphQL().WithOptions(new GraphQLServerOptions { Tool = { Enable = app.Environment.IsDevelopment() } });

app.Run();