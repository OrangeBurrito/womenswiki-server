using FluentValidation;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;

namespace WomensWiki.Features.Articles;

public static class GetArticleById {
    public record GetArticleByIdRequest(Guid Id) : IRequest<Result<ArticleResponse>>;

    public class GetArticleByIdValidator : AbstractValidator<GetArticleByIdRequest> {
        public GetArticleByIdValidator() {
            RuleFor(x => x.Id).ArticleExists();
        }
    }

    internal sealed class GetArticleByIdHandler(AppDbContext dbContext, GetArticleByIdValidator validator)
        : IRequestHandler<GetArticleByIdRequest, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(GetArticleByIdRequest request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.FindAsync(request.Id);

            var context = new ValidationContext<GetArticleByIdRequest>(request);
            context.RootContextData["Article"] = article;
            var validationResult = await validator.ValidateAsync(context);

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
