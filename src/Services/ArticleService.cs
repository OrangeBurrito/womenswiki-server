public class ArticleService {
    private WikiContext _context;

    public ArticleService(WikiContext wikiContext) {
        _context = wikiContext;
    }

    public async Task CreateArticleAsync(Article article) {
        article.Slug = Article.GenerateSlug(article.Title);
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        article.LatestRevisionId = article.Revisions.First().Id;
        article.LatestRevision = article.Revisions.First();
        await _context.SaveChangesAsync();
    }
}