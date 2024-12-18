using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Features.Articles.Persistence;

namespace WomensWiki.Features.Articles;

public static class GetArticles {
    public record GetArticlesRequest(int Limit = 10, int Offset = 0, bool Descending = false) : IRequest<List<ArticleResponse>>;

    internal sealed class GetArticlesHandler(IArticleRepository repository) : IRequestHandler<GetArticlesRequest, List<ArticleResponse>> {
        public async Task<List<ArticleResponse>> Handle(GetArticlesRequest request, CancellationToken cancellationToken) {
            var articles = await repository.GetArticles(request.Descending, request.Limit, request.Offset);

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