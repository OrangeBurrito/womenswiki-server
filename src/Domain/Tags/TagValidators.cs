using FluentValidation;
using FluentValidation.Results;
using WomensWiki.Common;

namespace WomensWiki.Domain.Tags;

public static class TagValidators {
    public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            var tag = context.Get<Tag>("Tag");
			if (tag?.Name == value) {
				var failure = new ValidationFailure("Name", $"Tag with that name already exists") {
					ErrorCode = "DuplicateName"
				};
				context.AddFailure(failure);
			}
        });
    }

    public static IRuleBuilderOptions<T, string> NoSpaces<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (value.Contains(" ") || value.Contains("-")) {
				var failure = new ValidationFailure("Name", $"Tag name cannot contain spaces or dashes") {
					ErrorCode = "SpaceInName"
				};
				context.AddFailure(failure);
            }
        });
    }

    public static IRuleBuilderOptions<T, string> TagExists<T>(this IRuleBuilder<T, string> ruleBuilder, string? property = "Tag") {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
           	var tag = context.Get<Tag>(property);
			var failure = new ValidationFailure(property, $"Tag does not exist: {value}") {
				ErrorCode = "TagDoesNotExist"
			};
			context.AddFailure(failure);
        });
    }

    public static IRuleBuilderOptions<T, List<string>> TagsExist<T>(this IRuleBuilder<T, List<string>> ruleBuilder) {
        return (IRuleBuilderOptions<T, List<string>>)ruleBuilder.Custom((value, context) => {
			var tags = context.Get<List<Tag>>("Tags");
            if (tags != null) {
                foreach (var tag in value) {
                    if (!tags.Any(t => t.Name == tag)) {
						var failure = new ValidationFailure("Tags", $"Tag does not exist: {tag}") {
							ErrorCode = "TagDoesNotExist"
						};
						context.AddFailure(failure);
                    }
                }
            }
        });
    }

    public static IRuleBuilderOptions<T, string> NotChildOfTag<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData["ParentIsChild"] == null) {
				var failure = new ValidationFailure("ParentTag", $"Parent tag cannot be a nested child of the tag") {
					ErrorCode = "ParentTagIsChild"
				};
				context.AddFailure(failure);
            }
        });
    }
}