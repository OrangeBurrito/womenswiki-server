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

    public static IRuleBuilderOptions<T, string> UniqueTitle<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Article"] != null) {
                var article = (Article)context.RootContextData["Article"];
                if (article.Title == value) {
                    var failure = new ValidationFailure("Title", $"Article with that title already exists");
                    failure.ErrorCode = "DuplicateTitle";
                    context.AddFailure(failure);
                }
            }
        });
    }
}