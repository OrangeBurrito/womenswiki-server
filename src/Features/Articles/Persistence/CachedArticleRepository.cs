using Microsoft.Extensions.Caching.Memory;
using WomensWiki.Domain;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features.Articles.Persistence;

public class CachedArticleRepository(ArticleRepository repository, IMemoryCache cache) : IArticleRepository {
    private static readonly TimeSpan CacheTime = TimeSpan.FromHours(1);
    private static readonly TimeSpan CacheTimeLong = TimeSpan.FromDays(2);

    public Task<Article> CreateArticle(string title, string content, List<Domain.Tags.Tag>? tags = null) {
        return repository.CreateArticle(title, content, tags);
    }

    public async Task<Article?> GetArticleById(Guid id) {
        return await cache.GetOrCreateAsync(
            $"article-id={id}",
            entry => {
                entry.SetAbsoluteExpiration(CacheTimeLong);
                return repository.GetArticleById(id);
            });
    }

    public async Task<Article?> GetArticleBySlug(string slug) {
        return await cache.GetOrCreateAsync(
            $"article-slug={slug}",
            entry => {
                entry.SetAbsoluteExpiration(CacheTimeLong);
                return repository.GetArticleBySlug(slug);
            });
    }

    public async Task<IEnumerable<Article>> GetArticles(bool descending, int limit, int offset) {
        return await cache.GetOrCreateAsync(
            $"articles-{(descending ? "desc" : "asc")}-max{limit}-offset{offset}",
            entry => {
                entry.SetAbsoluteExpiration(CacheTime);
                return repository.GetArticles(descending, limit, offset);
            });
    }

    public Task<List<Article>> GetArticlesByTag(Domain.Tags.Tag tag, bool descending, int limit, int offset) {
        return repository.GetArticlesByTag(tag, descending, limit, offset);
    }

    public Task<Article?> GetDuplicateArticle(string title) {
        return repository.GetDuplicateArticle(title);
    }

    public Task<Article> UpdateArticle(Article article, User user, string content, string? summary = null) {
        return repository.UpdateArticle(article, user, content, summary);
    }
}