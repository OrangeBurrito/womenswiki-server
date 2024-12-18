using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Features.Articles.Persistence;

namespace WomensWiki.Features.Articles;

public static class GetArticleById {
    public record GetArticleByIdRequest(Guid Id) : IRequest<Result<ArticleResponse>>;

    public class GetArticleByIdValidator : AbstractValidator<GetArticleByIdRequest> {
        public GetArticleByIdValidator() {
            RuleFor(x => x.Id).ArticleExists();
        }
    }

    internal sealed class GetArticleByIdHandler(IArticleRepository repository, GetArticleByIdValidator validator)
        : IRequestHandler<GetArticleByIdRequest, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(GetArticleByIdRequest request, CancellationToken cancellationToken) {
            var article = await repository.GetArticleById(request.Id);

            var validationResult = await validator.ValidateAsync(
                Validation.Context(request, ("Article", article))
            );

            if (!validationResult.IsValid) {
                return Result.Failure<ArticleResponse>(ErrorMapper.Map(validationResult));
            }
            
            return Result.Success(ArticleResponse.FromArticle(article));
        }
    }

    [QueryType]
    public class GetArticleByIdQuery {
        public async Task<Result<ArticleResponse>> GetArticleByIdAsync([Service] ISender sender, GetArticleByIdRequest input) {
            return await sender.Send(input);
        }
    }
}
