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

    public static IRuleBuilderOptions<T, string> TagExists<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Tag"] != null || context.RootContextData["ParentTag"] != null) {
                var tag = (Tag)context.RootContextData["Tag"] ?? (Tag)context.RootContextData["ParentTag"];
                if (tag.Name != value) {
                    var failure = new ValidationFailure("ParentTag", $"Tag does not exist: {value}");
                    failure.ErrorCode = "TagDoesNotExist";
                    context.AddFailure(failure);
                }
            }
        });
    }

    public static IRuleBuilderOptions<T, List<string>> TagsExist<T>(this IRuleBuilder<T, List<string>> ruleBuilder) {
        return (IRuleBuilderOptions<T, List<string>>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["Tags"] != null) {
                var tags = (List<Tag>)context.RootContextData["Tags"];
                foreach (var tag in value) {
                    if (!tags.Any(t => t.Name == tag)) {
                        var failure = new ValidationFailure("Tags", $"Tag does not exist: {tag}");
                        failure.ErrorCode = "TagDoesNotExist";
                        context.AddFailure(failure);
                    }
                }
            }
        });
    }
}