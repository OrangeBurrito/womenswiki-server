using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Features.Articles.Persistence;
using WomensWiki.Features.Users.Persistence;

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

    internal sealed class UpdateArticleHandler(IArticleRepository articleRepository, IUserRepository userRepository, UpdateArticleValidator validator)
        : IRequestHandler<UpdateArticleCommand, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken) {
            var article = await articleRepository.GetArticleById(request.ArticleId);
            var author = await userRepository.GetUserByUsername(request.Author);

            var validationResult = await validator.ValidateAsync(Validation.Context(request, ("Article", article)));
            if (!validationResult.IsValid) {
                return Result.Failure<ArticleResponse>(ErrorMapper.Map(validationResult));
            }

            await articleRepository.UpdateArticle(article, author, request.Content, request.Summary);
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