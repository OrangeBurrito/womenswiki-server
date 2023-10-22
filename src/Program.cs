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

app.MapGet("/articles", (WikiContext context) => {
    return context.Articles;
});

app.MapGet("/article/{id}", (WikiContext context, Guid id) => {
    Console.WriteLine(id);
    return context.Articles.Find(id);
});

app.MapPost("/article/create", async (WikiContext context, string Title) => {
    var article = new Article(Title);

    await context.Articles.AddAsync(article);
    await context.SaveChangesAsync();
    return article;
});

app.MapPatch("/article/update/{id}", async (WikiContext context, Guid id, string Title) => {
    var article = await context.Articles.FindAsync(id);
    article.Update(Title);
    await context.SaveChangesAsync();
    return article;
});

app.MapDelete("/article/delete/{id}", async (WikiContext context, Guid id) => {
    var article = await context.Articles.FindAsync(id);
    context.Articles.Remove(article);
    await context.SaveChangesAsync();
    return article;
});

app.Run();
