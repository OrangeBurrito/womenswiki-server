using FluentValidation;
using FluentValidation.Results;

namespace WomensWiki.Domain.Tags;

public static class TagValidators {
    public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Tag"] != null) {
                var tag = (Tag)context.RootContextData["Tag"];
                if (tag.Name == value) {
                    var failure = new ValidationFailure("Name", $"Tag with that name already exists");
                    failure.ErrorCode = "DuplicateName";
                    context.AddFailure(failure);
                }
            }
        });
    }

    public static IRuleBuilderOptions<T, string> NoSpaces<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (value.Contains(" ") || value.Contains("-")) {
                var failure = new ValidationFailure("Name", $"Tag name cannot contain spaces or dashes");
                failure.ErrorCode = "SpaceInName";
                context.AddFailure(failure);
            }
        });
    }

    public static IRuleBuilderOptions<T, List<string>> TagsExist<T>(this IRuleBuilder<T, List<string>> ruleBuilder) {
        return (IRuleBuilderOptions<T, List<string>>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Tags"] != null) {
                var tags = (List<Tag>)context.RootContextData["Tags"];
                if (value.Count != tags.Count) {
                    var failure = new ValidationFailure("Tags", $"Tags do not exist: {string.Join(", ", value)}");
                    failure.ErrorCode = "TagsDoNotExist";
                    context.AddFailure(failure);
                }
            }
        });
    }
}