using FluentValidation;
using FluentValidation.Results;

namespace WomensWiki.Domain.Users;

public static class UserValidators {
    public static IRuleBuilderOptions<T, Guid> UserExists<T>(this IRuleBuilder<T, Guid> ruleBuilder, string? property = "User") {
        return (IRuleBuilderOptions<T, Guid>)ruleBuilder.Custom((value, context) => {
            if (context.RootContextData[property] == null) {
                var failure = new ValidationFailure("UserId", $"User with that ID does not exist");
                failure.ErrorCode = "UserNotFound";
                context.AddFailure(failure);
            }
        });
    }
	public static IRuleBuilderOptions<T, string> UserExists<T>(this IRuleBuilder<T, string> ruleBuilder, string? property = "User") {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			if (context.RootContextData[property] == null) {
				var failure = new ValidationFailure("Username", $"User with that username does not exist");
				failure.ErrorCode = "UserNotFound";
				context.AddFailure(failure);
			}
		});
	}

	public static IRuleBuilderOptions<T, string> UniqueUsername<T>(this IRuleBuilder<T, string> ruleBuilder) {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			if (context.RootContextData["User"] != null) {
                var user = (User)context.RootContextData["User"];
				if (user.Username == value) {
					var failure = new ValidationFailure("Username", $"User with that username already exists");
					failure.ErrorCode = "DuplicateUsername";
					context.AddFailure(failure);
				}
			}
		});
	}

	public static IRuleBuilderOptions<T, string> UniqueEmail<T>(this IRuleBuilder<T, string> ruleBuilder) {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			if (context.RootContextData["User"] != null) {
				var user = (User)context.RootContextData["User"];
				if (user.Email == value) {
					var failure = new ValidationFailure("Email", $"User with that email already exists");
					failure.ErrorCode = "DuplicateEmail";
					context.AddFailure(failure);
				}
			}
		});
	}
}