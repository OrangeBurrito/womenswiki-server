using MediatR;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Articles;

public static class GetArticleById {
    public record GetArticleByIdRequest(Guid Id) : IRequest<ArticleResponse>;

    internal sealed class GetArticleByIdHandler(AppDbContext dbContext) : IRequestHandler<GetArticleByIdRequest, ArticleResponse> {
        public async Task<ArticleResponse> Handle(GetArticleByIdRequest request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.FindAsync(request.Id);
            
            return ArticleResponse.FromArticle(article);
        }
    }

    [QueryType]
    public class GetArticleByIdQuery {
        public async Task<ArticleResponse> GetArticleByIdAsync([Service] ISender sender, GetArticleByIdRequest input) {
            return await sender.Send(input);
        }
    }
}
