using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Domain;
using WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Articles.Persistence;

public class ArticleRepository(AppDbContext dbContext) : IArticleRepository {

    public async Task<Article?> GetArticleById(Guid id) {
        return await dbContext.Articles
            .Include(a => a.History)
            .Include(a => a.Tags).ThenInclude(t => t.Color).AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Article?> GetArticleByTitle(string title) {
        return await dbContext.Articles
            .Include(a => a.Tags).ThenInclude(t => t.Color).AsNoTracking()
            .FirstOrDefaultAsync(a => a.Title.ToLower() == Article.FormatTitle(title).ToLower());
    }

    public async Task<Article?> GetDuplicateArticle(string title) {
        return await dbContext.Articles.FirstOrDefaultAsync(a => a.Title == Article.FormatTitle(title));
    }

    public async Task<IEnumerable<Article>> GetArticles(bool descending, int limit, int offset) {
        var query = descending ?
            dbContext.Articles.OrderByDescending(a => a.CreatedAt) :
            dbContext.Articles.OrderBy(a => a.CreatedAt);

        return await query
            .Include(a => a.Tags).ThenInclude(t => t.Color).AsNoTracking()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Article>> GetArticlesByTag(Tag tag, bool descending, int limit, int offset) {
        var query = descending ?
            dbContext.Articles.OrderByDescending(a => a.CreatedAt) :
            dbContext.Articles.OrderBy(a => a.CreatedAt);

        return await query
            .Include(a => a.Tags)
            .Where(a => a.Tags.Contains(tag))
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Article> CreateArticle(User user, string title, string content, List<Tag>? tags = null) {
        var article = Article.Create(title);
        if (tags.Count > 0) {
            article.UpdateTags(tags);
            foreach (var tag in tags) {
                tag.AddArticle(article, tag);
            }
        }
        await dbContext.Articles.AddAsync(article);
        await dbContext.SaveChangesAsync();
        
        article.Update(user, content);
        await dbContext.SaveChangesAsync();
        
        return article;
    }

    public async Task<Article> UpdateArticle(Article article, User user, string content, string? summary = null) {
        dbContext.Articles.Attach(article);
        article.Update(user, content, summary);
        await dbContext.SaveChangesAsync();
        return article;
    }
}