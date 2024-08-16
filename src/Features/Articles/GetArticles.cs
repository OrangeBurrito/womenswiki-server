using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Articles;

public static class GetArticles {
    public record GetArticlesRequest(int Limit = 10, int Offset = 0, bool Descending = false) : IRequest<List<ArticleResponse>>;

    internal sealed class GetArticlesHandler(AppDbContext dbContext) : IRequestHandler<GetArticlesRequest, List<ArticleResponse>> {
        public async Task<List<ArticleResponse>> Handle(GetArticlesRequest request, CancellationToken cancellationToken) {
            var query = request.Descending ?
                dbContext.Articles.OrderByDescending(a => a.CreatedAt) :
                dbContext.Articles.OrderBy(a => a.CreatedAt);

            var articles = await query
                .Include(a => a.Tags)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return articles.Select(ArticleResponse.FromArticle).ToList();
        }
    }

    [QueryType]
    public class GetArticlesQuery {
        [UseSorting]
        public async Task<List<ArticleResponse>> GetArticlesAsync([Service] ISender sender, GetArticlesRequest input) {
            return await sender.Send(input);
        }
    }
}