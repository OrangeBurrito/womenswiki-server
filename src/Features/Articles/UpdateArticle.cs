using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features;

public static class UpdateArticle {
    public record UpdateArticleCommand(Guid ArticleId, string Author, string Content, string? Summary = null) : IRequest<ArticleResponse>;

    internal sealed class UpdateArticleHandler(AppDbContext dbContext) : IRequestHandler<UpdateArticleCommand, ArticleResponse> {
        public async Task<ArticleResponse> Handle(UpdateArticleCommand request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.Include(a => a.History).SingleAsync(a => a.Id == request.ArticleId);
            // if article
            if (article == null) {
                Console.WriteLine($"Article is null");
            } else {
                Console.WriteLine($"Article: {article.Id}");
            }
            var author = await dbContext.Users.SingleAsync(u => u.Username == request.Author);

            article.Update(author, request.Content, request.Summary);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Updated article");

            return ArticleResponse.FromArticle(article);
            Console.WriteLine("Returned response");
        }
    }

    [MutationType]
    public class UpdateArticleMutation {
        public async Task<ArticleResponse> UpdateArticleAsync([Service] ISender sender, UpdateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}