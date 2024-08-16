using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Articles;

public static class GetArticleBySlug {
    public record GetArticleBySlugRequest(string Slug) : IRequest<Result<ArticleResponse>>;
       internal sealed class GetArticleBySlugHandler(AppDbContext dbContext) : IRequestHandler<GetArticleBySlugRequest, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(GetArticleBySlugRequest request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Slug == request.Slug);

            if (article == null) {
                return Result.Failure<ArticleResponse>(new List<Error> { new Error("ArticleId", "Article not found")});
            }
            
            return Result.Success(ArticleResponse.FromArticle(article));
        }
    }

    [QueryType]
    public class GetArticleBySlugQuery {
        public async Task<Result<ArticleResponse>> GetArticleBySlugAsync([Service] ISender sender, string slug) {
            return await sender.Send(new GetArticleBySlugRequest(slug));
        }
    }
}