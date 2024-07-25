using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Articles;

public static class GetArticleBySlug {
    public record GetArticleBySlugRequest(string Slug) : IRequest<ArticleResponse>;
       internal sealed class GetArticleBySlugHandler(AppDbContext dbContext) : IRequestHandler<GetArticleBySlugRequest, ArticleResponse> {
        public async Task<ArticleResponse> Handle(GetArticleBySlugRequest request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.FirstOrDefaultAsync(a => a.Slug == request.Slug);
            
            return ArticleResponse.FromArticle(article);
        }
    }

    [QueryType]
    public class GetArticleBySlugQuery {
        public async Task<ArticleResponse> GetArticleBySlugAsync([Service] ISender sender, string slug) {
            return await sender.Send(new GetArticleBySlugRequest(slug));
        }
    }
}