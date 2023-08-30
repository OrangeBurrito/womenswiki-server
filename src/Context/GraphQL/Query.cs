using Microsoft.EntityFrameworkCore;

public class Query {
    public async Task<Article?> GetArticleAsync(WikiContext context, string slug) =>
        await context.Articles.Where(a => a.Slug == slug)
            .Include(a => a.LatestRevision)
            .FirstOrDefaultAsync();
}