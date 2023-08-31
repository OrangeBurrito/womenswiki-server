using Microsoft.EntityFrameworkCore;

public class ArticleService {
    private WikiContext _context;

    public ArticleService(WikiContext wikiContext) {
        _context = wikiContext;
    }

    public async Task<Article> CreateArticleAsync(ArticleDTO articleDTO) {
        var errors = new List<Error>();

        if (articleDTO.Title == null) {
            errors.Add(new Error("Title is required."));
        }
        if (await _context.Articles.AnyAsync(a => a.Title == articleDTO.Title)) {
            errors.Add(new Error("Title must be unique."));
        }

        if (articleDTO.Content == null || articleDTO.Content.Length < 100) {
            errors.Add(new Error("Content is required, and must be at least 100 characters long."));
        }

        if (errors.Count > 0) {
            throw new AggregateException(errors.Select(e => new ArgumentException(e.Message)));
        }

        var article = new Article();

        article.Title = articleDTO.Title;
        article.Slug = Article.GenerateSlug(articleDTO.Title);
        article.Revisions = new List<Revision> {
            new Revision {
                Content = articleDTO.Content
            }
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        article.LatestRevisionId = article.Revisions.First().Id;
        article.LatestRevision = article.Revisions.First();
        await _context.SaveChangesAsync();

        return article;
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