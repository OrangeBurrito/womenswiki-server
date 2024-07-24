using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain;

namespace WomensWiki.Features.Articles;

public static class CreateArticle {
    public record CreateArticleCommand(string Author, string Title, string Content) : IRequest<CreateArticleResponse>;

    internal sealed class CreateArticleHandler(AppDbContext dbContext) : IRequestHandler<CreateArticleCommand, CreateArticleResponse> {
        public async Task<CreateArticleResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken) {
            var author = await dbContext.Users.SingleAsync(u => u.Username == request.Author);

            var article = Article.Create(request.Title, request.Content);
            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();

            return new CreateArticleResponse(article.Id, article.CreatedAt, article.Title, article.Content);
        }
    }

    [MutationType]
    public class CreateArticleMutation {
        public async Task<CreateArticleResponse> CreateArticleAsync([Service] ISender sender, CreateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}