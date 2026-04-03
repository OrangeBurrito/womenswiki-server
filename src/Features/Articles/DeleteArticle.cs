using FluentValidation;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Common.Attributes;
using WomensWiki.Domain.Articles;
using WomensWiki.Features.Articles.Persistence;

namespace WomensWiki.Features.Articles;

public static class DeleteArticle {
	public record DeleteArticleCommand(Guid ArticleId) : IRequest<Result<DeleteArticleResponse>>;
	public record DeleteArticleResponse(Guid ArticleId, DateTimeOffset DeletedAt);

	public sealed class DeleteArticleValidator : AbstractValidator<DeleteArticleCommand> {
		public DeleteArticleValidator() {
			RuleFor(x => x.ArticleId).NotEmpty().ArticleExists();
		}
	}

	internal sealed class DeleteArticleHandler(
		IArticleRepository articleRepository,
		DeleteArticleValidator validator
	) : IRequestHandler<DeleteArticleCommand, Result<DeleteArticleResponse>> {
		public async Task<Result<DeleteArticleResponse>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken) {
			var article = await articleRepository.GetArticleById(request.ArticleId);

			var validationResult = await validator.ValidateAsync(Validation.Context(request, ("Article", article)));
			if (!validationResult.IsValid) {
				return Result.Failure<DeleteArticleResponse>(ErrorMapper.Map(validationResult));
			}

			await articleRepository.DeleteArticle(article!);

			return Result.Success(new DeleteArticleResponse(article!.Id, DateTimeOffset.UtcNow));
		}
	}

	[MutationType]
	public class DeleteArticleMutation {
		[RoleAuthorize(Constants.Role.Admin)]
		public async Task<Result<DeleteArticleResponse>> DeleteArticleAsync([Service] ISender sender, DeleteArticleCommand input) {
			return await sender.Send(input);
		}
	}
}