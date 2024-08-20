using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Tags;

namespace WomensWiki.Features.Articles;

public static class UpdateArticleTags {
    public record UpdateArticleTagsCommand(Guid ArticleId, string Author, List<string> Tags) : IRequest<Result<ArticleResponse>>;

    public class UpdateArticleTagsValidator : AbstractValidator<UpdateArticleTagsCommand> {
        public UpdateArticleTagsValidator() {
            RuleFor(x => x.ArticleId).ArticleExists();
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Tags).NotEmpty().TagsExist();
        }
    }

    public class UpdateArticleTagsHandler(AppDbContext dbContext, UpdateArticleTagsValidator validator)
        : IRequestHandler<UpdateArticleTagsCommand, Result<ArticleResponse>> {
        public async Task<Result<ArticleResponse>> Handle(UpdateArticleTagsCommand request, CancellationToken cancellationToken) {
            var article = await dbContext.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == request.ArticleId);
            var tags = await dbContext.Tags.Where(t => request.Tags.Contains(t.Name)).ToListAsync();

            var context = new ValidationContext<UpdateArticleTagsCommand>(request);
            context.RootContextData["Article"] = article;
            context.RootContextData["Tags"] = tags;
            var validationResult = await validator.ValidateAsync(context);

            if (!validationResult.IsValid) {
                return Result.Failure<ArticleResponse>(ErrorMapper.Map(validationResult));
            }

            article.UpdateTags(tags);
            foreach (var tag in tags) {
                if (!tag.Articles.Contains(article)) {
                    tag.AddArticle(article, tag);
                }
            }
            await dbContext.SaveChangesAsync();

            return Result.Success(ArticleResponse.FromArticle(article));
        }
    }

    [MutationType]
    public class UpdateArticleTagsMutation {
        public async Task<Result<ArticleResponse>> UpdateArticleTags([Service] ISender sender, UpdateArticleTagsCommand input) {
            return await sender.Send(input);
        }
    }
}