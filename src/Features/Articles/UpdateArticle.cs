using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features;

public static class UpdateArticle {
    public record UpdateArticleCommand(Guid ArticleId, string Author, string Content, string? Summary = null) : IRequest<Result<ArticleResponse>>;

    public class UpdateArticleValidator : AbstractValidator<UpdateArticleCommand> {
        public UpdateArticleValidator() {
            RuleFor(x => x.ArticleId).ArticleExists();
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Content).NotEmpty().MinimumLength(25);
        }
    }

    internal sealed class UpdateArticleHandler(AppDbContext dbContext, UpdateArticleValidator validator)
        : IRequestHandler<UpdateArticleCommand, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.Include(a => a.History).SingleAsync(a => a.Id == request.ArticleId);
            var author = await dbContext.Users.SingleAsync(u => u.Username == request.Author);

            var context = new ValidationContext<UpdateArticleCommand>(request);
            context.RootContextData["Article"] = article;
            var validationResult = await validator.ValidateAsync(context);

            if (!validationResult.IsValid) {
                return Result.Failure<ArticleResponse>(ErrorMapper.Map(validationResult));
            }

            article.Update(author, request.Content, request.Summary);
            await dbContext.SaveChangesAsync();

            return Result.Success(ArticleResponse.FromArticle(article));
        }
    }

    [MutationType]
    public class UpdateArticleMutation {
        public async Task<Result<ArticleResponse>> UpdateArticleAsync([Service] ISender sender, UpdateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}