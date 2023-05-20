using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<WikiContext>(options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
    })
    .AddCors(options => {
        IConfiguration config = builder.Configuration;
        options.AddDefaultPolicy(builder => builder.WithOrigins(config.GetSection("AllowedOrigins").Value)
        .AllowAnyMethod().AllowAnyHeader());
    });

var app = builder.Build();
app.UseCors();

using (var scope = app.Services.CreateScope()) {
    var wikiContext = scope.ServiceProvider.GetRequiredService<WikiContext>();
    wikiContext.Database.EnsureDeleted();
    wikiContext.Database.EnsureCreated();

    wikiContext.Articles.Add(new Article { Title = "First Article"});
    wikiContext.Articles.Add(new Article { Title = "Second Article"});

    wikiContext.SaveChanges();
}

app.MapGet("/articles", async (WikiContext context) => {
    var articles = await context.Articles.ToListAsync();
    return articles;
});

app.MapGet("/search", async (WikiContext context, [FromQuery] string title) => {
    var articles = await context.Articles.Where(a => a.Slug.Contains(title)).ToListAsync();
    return articles;
});

// app.MapGet("/articles/{slug}", async (WikiContext context, string slug) => {
//     return await context.Articles.Where(a => a.Slug == slug).FirstOrDefaultAsync();
// });

app.Run();