namespace Context.GraphQL;

public class MutationResult<T> {
    public T? Payload { get; set; } = default!;
    public List<Error> Errors { get; set; } = new();
}