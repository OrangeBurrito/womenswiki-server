using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Common.Interfaces;
using WomensWiki.Domain;
using WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Articles.Persistence;

public class ArticleRepository(AppDbContext dbContext) : IRepository {

    public async Task<Article?> GetArticleById(Guid id) {
        return await dbContext.Articles
            .Include(a => a.History)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Article?> GetArticleBySlug(string slug) {
        return await dbContext.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Slug == slug);
    }

    public async Task<Article?> GetDuplicateArticle(string title) {
        return await dbContext.Articles.FirstOrDefaultAsync(a => a.Title == title && a.Slug == Article.GenerateSlug(title));
    }

    public async Task<IEnumerable<Article>> GetArticles(bool descending, int limit, int offset) {
        var query = descending ?
            dbContext.Articles.OrderByDescending(a => a.CreatedAt) :
            dbContext.Articles.OrderBy(a => a.CreatedAt);

        return await query
            .Include(a => a.Tags)
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

    public async Task<Article> CreateArticle(string title, string content, List<Tag>? tags = null) {
        var article = Article.Create(title, content);
        if (tags.Count > 0) {
            article.UpdateTags(tags);
            foreach (var tag in tags) {
                tag.AddArticle(article, tag);
            }
        }
        await dbContext.Articles.AddAsync(article);
        await dbContext.SaveChangesAsync();
        return article;
    }

    public async Task<Article> UpdateArticle(Article article, User user, string content, string? summary = null) {
        article.Update(user, content, summary);
        await dbContext.SaveChangesAsync();
        return article;
    }
}