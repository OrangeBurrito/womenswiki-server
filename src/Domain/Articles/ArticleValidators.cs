using FluentValidation;
using FluentValidation.Results;

namespace WomensWiki.Domain.Articles;

public static class ArticleValidators {
    public static IRuleBuilderOptions<T, Guid> ArticleExists<T>(this IRuleBuilder<T, Guid> ruleBuilder) {
        return (IRuleBuilderOptions<T, Guid>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Article"] == null) {
                var failure = new ValidationFailure("ArticleId", "Article with that ID does not exist");
                failure.ErrorCode = "ArticleNotFound";
                context.AddFailure(failure);
            }
        });
    }
}