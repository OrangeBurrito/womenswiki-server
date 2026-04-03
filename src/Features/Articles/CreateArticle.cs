using FluentValidation;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Common.Attributes;
using WomensWiki.Common.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Articles.Persistence;
using WomensWiki.Features.Tags.Persistence;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Features.Articles;

public static class CreateArticle {
    public record CreateArticleCommand(Guid Author, string Title, string Content, List<string>? Tags = null) : IRequest<Result<CreateArticleResponse>>;

    public sealed class CreateArticleValidator : AbstractValidator<CreateArticleCommand> {
        public CreateArticleValidator() {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().UniqueTitle().MinimumLength(3);
            RuleFor(x => x.Content).NotEmpty().MinimumLength(25);
            When(x => x.Tags != null, () => {
                RuleFor(x => x.Tags).TagsExist();
            });
        }
    }

    internal sealed class CreateArticleHandler(
        IArticleRepository articleRepository,
        ITagRepository tagRepository,
        IUserRepository userRepository,
        CreateArticleValidator validator
    )
    : IRequestHandler<CreateArticleCommand, Result<CreateArticleResponse>> {
        public async Task<Result<CreateArticleResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken) {
            var author = await userRepository.GetUserById(request.Author);
            var tags = await tagRepository.GetMatchingTags(request.Tags);
            var duplicateArticle = await articleRepository.GetDuplicateArticle(request.Title);

            var validationResult = await validator.ValidateAsync(
                Validation.Context(request, ("Article", duplicateArticle), ("Tags", tags))
            );
            if (!validationResult.IsValid) {
                return Result.Failure<CreateArticleResponse>(ErrorMapper.Map(validationResult));
            }

            var article = await articleRepository.CreateArticle(author, request.Title, request.Content, tags);

            return Result.Success(new CreateArticleResponse(article.Id, article.CreatedAt, article.Title, article.LatestVersion, article.Tags));
        }
    }

    [MutationType]
    public class CreateArticleMutation {
		[RoleAuthorize(Constants.Role.Editor | Constants.Role.Admin)]
        public async Task<Result<CreateArticleResponse>> CreateArticleAsync([Service] ISender sender, CreateArticleCommand input) {
            return await sender.Send(input);
        }
    }
}