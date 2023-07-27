using Microsoft.EntityFrameworkCore;

public class ArticleService {
    private WikiContext _context;

    public ArticleService(WikiContext wikiContext) {
        _context = wikiContext;
    }

    public async Task CreateArticleAsync(Article article) {
        if (article.Title == null) {
            throw new ArgumentException("Title is required.");
        }
        if (await _context.Articles.AnyAsync(a => a.Title == article.Title)) {
            throw new ArgumentException("Title must be unique.");
        }

        if (article.Revisions.First().Content == null || article.Revisions.First().Content.Length < 100) {
            throw new ArgumentException("Content is required, and must be at least 100 characters long.");
        }

        article.Slug = Article.GenerateSlug(article.Title);
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        article.LatestRevisionId = article.Revisions.First().Id;
        article.LatestRevision = article.Revisions.First();
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArticleAsync(Revision revision) {
        if (revision.Content == null || revision.Content.Length < 100) {
            throw new ArgumentException("Content is required, and must be at least 100 characters long.");
        }

        var article = _context.Articles
            .Where(a => a.Id == revision.ArticleId)
            .Include(a => a.Revisions)
            .FirstOrDefault();

        if (article == null) {
            throw new ArgumentException("Article with the provided ID does not exist.");
        }

        article.LatestRevisionId = revision.Id;
        article.LatestRevision = revision;
        article.Revisions.Add(revision);

        _context.Articles.Update(article);
        _context.Revisions.Add(revision);
        await _context.SaveChangesAsync();
    }
}