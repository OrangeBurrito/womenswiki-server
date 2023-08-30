using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class EndpointBuilder {
    public static void AddApiEndpoints(this WebApplication app) {
        app.MapPost("/register", () => {
            return "Register!";
        });
        app.MapPost("/login", (HttpContext context) => {
            return "login!";
        });

        app.MapPost("/seed", async (SeedService seedService) => {
            await seedService.Seed();
            return Results.Ok("Seeded Data");
        });

        app.MapGet("/articles", async (WikiContext context) => {
            var articles = await context.Articles.Include(a => a.Revisions).ToListAsync();
            return articles;
        });

        app.MapGet("/revisions", async (WikiContext context) => {
            var revisions = await context.Revisions.ToListAsync();
            return revisions;
        });

        app.MapGet("/search", async (WikiContext context, [FromQuery] string title) => {
            var articles = await context.Articles.Where(a => a.Slug.Contains(Article.GenerateSlug(title))).ToListAsync();
            return articles;
        });

        app.MapGet("/article/{slug}", async (WikiContext context, string slug) => {
            return await context.Articles
                .Where(a => a.Slug == slug)
                .Include(a => a.Revisions)
                .FirstOrDefaultAsync();
        });

        app.MapGet("/article/{id:guid}", async (WikiContext context, Guid id) => {
            return await context.Articles
                .Where(a => a.Id == id)
                .Include(a => a.Revisions)
                .FirstOrDefaultAsync();
        });

        app.MapPost("/create_article", async (Article article, ArticleService articleService) => {
            await articleService.CreateArticleAsync(article);
            return Results.Created($"/article/{article.Slug}", article);
        });

        app.MapPost("/update_article", async (Revision revision, WikiContext context, ArticleService articleService) => {
            await articleService.UpdateArticleAsync(revision);
            return Results.Created($"/article/{revision.ArticleId}", revision);
        });
    }
}