using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Features.Articles.Persistence;

namespace WomensWiki.Features.Articles;

public static class GetArticleByTitle {
    public record GetArticleByTitleRequest(string Title) : IRequest<Result<ArticleResponse>>;
       internal sealed class GetArticleByTitleHandler(IArticleRepository repository) : IRequestHandler<GetArticleByTitleRequest, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(GetArticleByTitleRequest request, CancellationToken cancellationToken) {
            var article = await repository.GetArticleByTitle(request.Title);

            if (article == null) {
                return Result.Failure<ArticleResponse>(new List<Error> { new Error("ArticleId", "Article not found")});
            }
            
            return Result.Success(ArticleResponse.FromArticle(article));
        }
    }

    [QueryType]
    public class GetArticleByTitleQuery {
        public async Task<Result<ArticleResponse>> GetArticleByTitleAsync([Service] ISender sender, string title) {
            return await sender.Send(new GetArticleByTitleRequest(title));
        }
    }
}