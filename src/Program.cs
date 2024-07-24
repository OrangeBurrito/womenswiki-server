using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Environment.IsDevelopment() ? "DevConnection" : "AzureConnection";

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddGraphQLServer().AddTypes().AddSorting();

var app = builder.Build();

app.MapGraphQL();

app.MapPost("/migrate", async (AppDbContext dbContext) => {
    await dbContext.Database.MigrateAsync();
    return Results.Ok();
});

app.Run();