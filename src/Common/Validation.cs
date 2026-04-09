using FluentValidation;

namespace WomensWiki.Common;

public static class Validation {
    public static ValidationContext<T> Context<T>(T instance, params (string propertyName, object? property)[] properties) {
        var context = new ValidationContext<T>(instance);
        foreach (var (propertyName, property) in properties) {
            context.RootContextData[propertyName] = property;
        }
        return context;
    }

	public static T? Get<T>(this IValidationContext context, string key) {
		if (context.RootContextData.TryGetValue(key, out var value) && value is T typedValue) {
			return typedValue;
		}
		return default;
	}
}