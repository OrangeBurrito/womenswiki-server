using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using FluentValidation;
using HotChocolate.AspNetCore;
using WomensWiki.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Environment.IsDevelopment() ? "DevConnection" : "AzureConnection";
var assembly = typeof(Program).Assembly;
// refactor later
var repositoryTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract)
    .Select(t => new {
        Implementation = t,
        Interface = t.GetInterfaces().FirstOrDefault(i => typeof(IRepository).IsAssignableFrom(i) && i != typeof(IRepository))
    })
    .Where(t => t.Interface != null);
var cachedRepositoryTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract).Where(t => t.Name.StartsWith("Cached"))
    .Where(t => t.GetInterfaces().Any(i => typeof(IRepository).IsAssignableFrom(i) && i != typeof(IRepository)))
    .ToList();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));

foreach (var type in repositoryTypes) {
    var cached = cachedRepositoryTypes.FirstOrDefault(t => t.GetInterfaces().Contains(type.Interface));
    if (cached != null) {
        builder.Services.AddScoped(type.Interface, cached);
        builder.Services.AddScoped(type.Implementation);
    } else {
        builder.Services.AddScoped(type.Interface, type.Implementation);
    }
}
builder.Services.AddMemoryCache();
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