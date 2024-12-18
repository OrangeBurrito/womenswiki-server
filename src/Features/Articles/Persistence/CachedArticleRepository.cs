using Microsoft.Extensions.Caching.Memory;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features.Articles.Persistence;

public class CachedArticleRepository(IArticleRepository repository, IMemoryCache cache) {
public async Task<Article?> GetArticleById(Guid id) {
        return await cache.GetOrCreateAsync(
            id,
            entry => repository.GetArticleById(id));
    }
}