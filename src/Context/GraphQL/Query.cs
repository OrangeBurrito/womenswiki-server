using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

public class Query {
    public async Task<Article?> GetArticleAsync(WikiContext context, string slug) =>
        await context.Articles.Where(a => a.Slug == slug)
            .Include(a => a.LatestRevision)
            .FirstOrDefaultAsync();

    [UseSorting]
    public async Task<List<Article>> GetArticlesAsync(WikiContext context, int first) =>
        await context.Articles.Include(a => a.LatestRevision).Take(first).ToListAsync();
}