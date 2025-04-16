using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore;
using WomensWiki;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("WikiConnection")!, assembly);
builder.Services.AddApplication(assembly);
builder.Services.AddApi();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseCors("Localhost");
}
app.MapGraphQL().WithOptions(new GraphQLServerOptions { Tool = { Enable = app.Environment.IsDevelopment() } });

app.Run();