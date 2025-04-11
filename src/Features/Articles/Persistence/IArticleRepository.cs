using WomensWiki.Common.Interfaces;
using WomensWiki.Domain;
using WomensWiki.Domain.Articles;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Articles.Persistence;

public interface IArticleRepository : IRepository {
    Task<Article?> GetArticleById(Guid id);
    // Task<Article?> GetArticleBySlug(string slug);
    Task<Article?> GetDuplicateArticle(string title);
    Task<IEnumerable<Article>> GetArticles(bool descending, int limit, int offset);
    Task<List<Article>> GetArticlesByTag(Tag tag, bool descending, int limit, int offset);
    Task<Article> CreateArticle(User user, string title, string content, List<Tag>? tags = null);
    Task<Article> UpdateArticle(Article article, User user, string content, string? summary = null);
}