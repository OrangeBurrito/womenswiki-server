namespace Context.GraphQL;

public class Error {
    public string Message { get; set; } = null!;

}

public static class ErrorExtension {
    public static void Add(this List<Error> errors, string message) {
        errors.Add(new Error { Message = message});
    }
}