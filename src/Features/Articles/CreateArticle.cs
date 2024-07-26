using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features.Articles;

public static class CreateArticle {
    public record CreateArticleCommand(string Author, string Title, string Content) : IRequest<Result<CreateArticleResponse>>;

    public class CreateArticleValidator : AbstractValidator<CreateArticleCommand> {
        public CreateArticleValidator() {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().UniqueTitle().MinimumLength(3);
            RuleFor(x => x.Content).NotEmpty().MinimumLength(25);
        }
    }

    internal sealed class CreateArticleHandler(AppDbContext dbContext, CreateArticleValidator validator)
        : IRequestHandler<CreateArticleCommand, Result<CreateArticleResponse>> {
        public async Task<Result<CreateArticleResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken) {
            var author = await dbContext.Users.SingleAsync(u => u.Username == request.Author);
            var duplicateArticle = await dbContext.Articles.FirstOrDefaultAsync(a => a.Title == request.Title);

            var context = new ValidationContext<CreateArticleCommand>(request);
            context.RootContextData["Article"] = duplicateArticle;
            var validationResult = await validator.ValidateAsync(context);

            if (!validationResult.IsValid) {
                return Result.Failure<CreateArticleResponse>(ErrorMapper.Map(validationResult));
            }

            var article = Article.Create(request.Title, request.Content);
            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();

            return Result.Success(new CreateArticleResponse(article.Id, article.CreatedAt, article.Title, article.Content, article.Slug));
        }
    }

    [MutationType]
    public class CreateArticleMutation {
        public async Task<Result<CreateArticleResponse>> CreateArticleAsync([Service] ISender sender, CreateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}