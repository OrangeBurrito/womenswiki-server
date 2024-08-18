using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Environment.IsDevelopment() ? "DevConnection" : "AzureConnection";

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCors(o => o.AddPolicy("Localhost", p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
builder.Services.AddCors(o => o.AddPolicy("Wiki", p => { p.WithOrigins("https://www.womenswiki.org").AllowAnyMethod().AllowAnyHeader(); }));
builder.Services.AddGraphQLServer().AddTypes().AddSorting();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseCors("Localhost");
} else {
    app.UseCors("Wiki");
}

app.MapGraphQL();

app.Run();