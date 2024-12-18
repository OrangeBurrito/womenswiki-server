using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Articles.Persistence;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Articles;

public static class CreateArticle {
    public record CreateArticleCommand(string Author, string Title, string Content, List<string>? Tags = null) : IRequest<Result<CreateArticleResponse>>;

    public class CreateArticleValidator : AbstractValidator<CreateArticleCommand> {
        public CreateArticleValidator() {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().UniqueTitle().MinimumLength(3);
            RuleFor(x => x.Content).NotEmpty().MinimumLength(25);
            When(x => x.Tags != null, () => {
                RuleFor(x => x.Tags).TagsExist();
            });
        }
    }

    internal sealed class CreateArticleHandler(IArticleRepository articleRepository, ITagRepository tagRepository, CreateArticleValidator validator)
        : IRequestHandler<CreateArticleCommand, Result<CreateArticleResponse>> {
        public async Task<Result<CreateArticleResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken) {
            var tags = await tagRepository.GetMatchingTags(request.Tags);
            var duplicateArticle = await articleRepository.GetDuplicateArticle(request.Title);

            var validationResult = await validator.ValidateAsync(
                Validation.Context(request, ("Article", duplicateArticle), ("Tags", tags))
            );
            if (!validationResult.IsValid) {
                return Result.Failure<CreateArticleResponse>(ErrorMapper.Map(validationResult));
            }

            var article = await articleRepository.CreateArticle(request.Title, request.Content, tags);

            return Result.Success(new CreateArticleResponse(article.Id, article.CreatedAt, article.Title, article.Content, article.Slug, article.Tags));
        }
    }

    [MutationType]
    public class CreateArticleMutation {
        public async Task<Result<CreateArticleResponse>> CreateArticleAsync([Service] ISender sender, CreateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}