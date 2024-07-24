using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain;

namespace WomensWiki.Features;

public static class UpdateArticle {
    public record UpdateArticleCommand(Guid ArticleId, string Author, string Content, string? Summary = null) : IRequest<ArticleResponse>;

    internal sealed class UpdateArticleHandler(AppDbContext dbContext) : IRequestHandler<UpdateArticleCommand, ArticleResponse> {
        public async Task<ArticleResponse> Handle(UpdateArticleCommand request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.Include(a => a.History).SingleAsync(a => a.Id == request.ArticleId);
            var author = await dbContext.Users.SingleAsync(u => u.Username == request.Author);

            article.Update(author, request.Content, request.Summary);
            await dbContext.SaveChangesAsync();

            return ArticleResponse.FromArticle(article);
        }
    }

    [MutationType]
    public class UpdateArticleMutation {
        public async Task<ArticleResponse> UpdateArticleAsync([Service] ISender sender, UpdateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}