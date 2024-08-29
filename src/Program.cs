using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using FluentValidation;
using HotChocolate.AspNetCore;
using WomensWiki.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Environment.IsDevelopment() ? "DevConnection" : "AzureConnection";
var repositoryTypes = assembly.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof(IRepository).IsAssignableFrom(t));

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));
foreach (var type in repositoryTypes) builder.Services.AddScoped(type);
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
builder.Services.AddCors(o => o.AddPolicy("Localhost", p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
builder.Services.AddGraphQLServer().AddTypes().AddSorting();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseCors("Localhost");
}

app.MapGraphQL().WithOptions(new GraphQLServerOptions { Tool = { Enable = app.Environment.IsDevelopment() } });

app.Run();