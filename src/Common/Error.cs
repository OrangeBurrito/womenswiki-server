
using FluentValidation.Results;

namespace WomensWiki;

public class Error {
    public string Code { get; private set; } = null!;
    public string? Message { get; private set; }

    public Error(string code, string? message = null) {
        Code = code;
        Message = message;
    }
}

public static class ErrorMapper {
    public static List<Error> Map(ValidationResult validationResult) {
        return validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)).ToList();
    }
}