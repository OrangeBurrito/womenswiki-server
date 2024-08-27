using FluentValidation;
using WomensWiki.Features.Tags;

namespace WomensWiki.Common.Validation;

public static class Validation {
    public static ValidationContext<T> Context<T>(T instance, params (string propertyName, object? property)[] properties) {
        var context = new ValidationContext<T>(instance);
        foreach (var (propertyName, property) in properties) {
            context.RootContextData[propertyName] = property;
        }
        return context;
    }
}