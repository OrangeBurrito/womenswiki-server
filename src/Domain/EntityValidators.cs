using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;

namespace WomensWiki.Domain;

public static class EntityValidators {
    public static IRuleBuilderOptions<T, Guid> NoSpecialCharacters<T>(this IRuleBuilder<T, Guid> ruleBuilder) {
        return (IRuleBuilderOptions<T, Guid>)ruleBuilder.Custom((value, context) => {
            if (!Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9]*$")) {
                var failure = new ValidationFailure("Value", "Value cannot contain special characters");
                failure.ErrorCode = "SpecialCharacters";
                context.AddFailure(failure);
            }
        });
    }
}