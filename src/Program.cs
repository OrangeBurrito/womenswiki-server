using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WikiContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddCors(options => {
    options.AddPolicy("WikiPolicy", policy => {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        if (allowedOrigins != null) {
            policy.WithOrigins(allowedOrigins);
        } else {
            policy.AllowAnyOrigin();
        }
        policy.AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("WikiPolicy");

app.MapGet("/", () => "Hello World!");

app.MapGet("/articles", (WikiContext wikiContext) => {
    return wikiContext.Articles;
});

app.MapGet("/article/{id}", (WikiContext wikiContext, Guid id) => {
    Console.WriteLine(id);
    return wikiContext.Articles.Find(id);
});

app.MapPost("/article/create", async (WikiContext wikiContext, HttpContext httpContext) => {
    var title = httpContext.Request.Form["title"].ToString();
    var article = new Article(title);

    await wikiContext.Articles.AddAsync(article);
    await wikiContext.SaveChangesAsync();
    return article;
});

app.MapPatch("/article/update/{id}", async (WikiContext wikiContext, Guid id, string Title) => {
    var article = await wikiContext.Articles.FindAsync(id);
    article.Update(Title);
    await wikiContext.SaveChangesAsync();
    return article;
});

app.MapDelete("/article/delete/{id}", async (WikiContext wikiContext, Guid id) => {
    var article = await wikiContext.Articles.FindAsync(id);
    wikiContext.Articles.Remove(article);
    await wikiContext.SaveChangesAsync();
    return article;
});

app.Run();
