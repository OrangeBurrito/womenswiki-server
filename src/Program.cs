using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WikiContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

var app = builder.Build();

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

app.MapGet("/articles/{slug}", async (WikiContext context, string slug) => {
    return await context.Articles.Where(a => a.Slug == slug).FirstOrDefaultAsync();
});

app.Run();