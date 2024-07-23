using MediatR;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain;

namespace WomensWiki.Features.Articles;

public static class CreateArticle {
    public record CreateArticleCommand(string Title, string Content) : IRequest<ArticleResponse>;

    internal sealed class CreateArticleHandler(AppDbContext dbContext) : IRequestHandler<CreateArticleCommand, ArticleResponse> {
        public async Task<ArticleResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken) {
            var article = Article.Create(request.Title, request.Content);
            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();

            return new ArticleResponse(article.Id, article.CreatedAt, article.Title, article.Content);
        }
    }

    [MutationType]
    public class CreateArticleMutation {
        public async Task<ArticleResponse> CreateArticleAsync([Service] ISender sender, CreateArticleCommand input) {
            return await sender.Send(input);
        }
    } 
}