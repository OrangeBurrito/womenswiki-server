public static class EndpointBuilder {
    public static void MapEndpoints(this WebApplication app) {     
        app.MapGet("/", () => "Hello World!");

        app.MapGet("/articles", async (WikiContext wikiContext) => {
            var articles = await wikiContext.Articles.Include(a => a.LatestRevision).ToListAsync();
            return articles.Select(a => new ArticleDTO(a.Id, a.Title, a.LatestRevision.Content));
        });

        app.MapGet("/article/{id}", (WikiContext wikiContext, Guid id) => {
            var article = wikiContext.Articles.Include(a => a.LatestRevision).FirstOrDefault(a => a.Id == id);
            return new ArticleDTO(article.Id, article.Title, article.LatestRevision.Content);
        });

        app.MapPost("/article/create", async (WikiContext wikiContext, HttpContext httpContext) => {
            var article = new Article(httpContext.Request.Form["title"]);

            await wikiContext.Articles.AddAsync(article);
            await wikiContext.SaveChangesAsync();

            var initialRevision = new Revision(article, httpContext.Request.Form["content"]);
            article.Update(initialRevision);

            await wikiContext.Revisions.AddAsync(initialRevision);
            await wikiContext.SaveChangesAsync();

            return new ArticleDTO(article.Id, article.Title, article.LatestRevision.Content);
        });

        app.MapPatch("/article/update", async (WikiContext wikiContext, HttpContext httpContext) => {
            var article = await wikiContext.Articles.FindAsync(Guid.Parse(httpContext.Request.Form["id"].ToString()));
            var revision = new Revision(article, httpContext.Request.Form["content"]);

            article.Update(revision);
            await wikiContext.Revisions.AddAsync(revision);

            await wikiContext.SaveChangesAsync();

            return new ArticleDTO(article.Id, article.Title, article.LatestRevision.Content);
        });

        app.MapDelete("/article/delete/{id}", async (WikiContext wikiContext, Guid id) => {
            var article = await wikiContext.Articles.FindAsync(id);
            // todo: remove matching revisions/mark as deleted
            wikiContext.Articles.Remove(article);
            await wikiContext.SaveChangesAsync();
            return article;
        });
    }
}