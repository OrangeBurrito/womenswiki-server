using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WikiContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/articles", (WikiContext context) => {
    return context.Articles;
});

app.Run();
