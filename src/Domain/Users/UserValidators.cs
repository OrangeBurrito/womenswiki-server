using FluentValidation;
using FluentValidation.Results;
using WomensWiki.Common;

namespace WomensWiki.Domain.Users;

public static class UserValidators {
    public static IRuleBuilderOptions<T, Guid> UserExists<T>(this IRuleBuilder<T, Guid> ruleBuilder, string? property = "User") {
        return (IRuleBuilderOptions<T, Guid>)ruleBuilder.Custom((value, context) => {
            if (context.Get<User>(property) == null) {
				var failure = new ValidationFailure("UserId", $"User with that ID does not exist") {
					ErrorCode = "UserNotFound"
				};
				context.AddFailure(failure);
            }
        });
    }
	public static IRuleBuilderOptions<T, string> UserExists<T>(this IRuleBuilder<T, string> ruleBuilder, string? property = "User") {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			if (context.Get<User>(property) == null) {
				var failure = new ValidationFailure(property, $"User with that username does not exist") {
					ErrorCode = "UserNotFound"
				};
				context.AddFailure(failure);
			}
		});
	}

	public static IRuleBuilderOptions<T, string> UniqueUsername<T>(this IRuleBuilder<T, string> ruleBuilder) {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			var user = context.Get<User>("User");
			if (user?.Username == value) {
				var failure = new ValidationFailure("Username", $"User with that username already exists") {
					ErrorCode = "DuplicateUsername"
				};
				context.AddFailure(failure);
			}
		});
	}

	public static IRuleBuilderOptions<T, string> UniqueEmail<T>(this IRuleBuilder<T, string> ruleBuilder) {
		return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) => {
			var user = context.Get<User>("Email");
			if (user?.Email == value) {
				var failure = new ValidationFailure("Email", $"User with that email already exists") {
					ErrorCode = "DuplicateEmail"
				};
				context.AddFailure(failure);
			}
		});
	}
}