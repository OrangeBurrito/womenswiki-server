using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateArticleAsync(Revision revision) {
        var article = _context.Articles
            .Where(a => a.Id == revision.ArticleId)
            .Include(a => a.Revisions)
            .FirstOrDefault();

        if (article == null) {
            throw new ArgumentException("Article with the provided ID does not exist.");
        } else {
            article.LatestRevisionId = revision.Id;
            article.LatestRevision = revision;
            article.Revisions.Add(revision);

            _context.Articles.Update(article);
            _context.Revisions.Add(revision);
            await _context.SaveChangesAsync();
        }
    }
}