using WomensWiki.Contracts;

namespace WomensWiki;

public static class Result {
    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
    public static Result<T> Failure<T>(List<Error> errors) => Result<T>.Failure(errors);
}

public class Result<T> {
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;

    public T? Data { get; private set; }
    public List<Error>? Errors { get; private set; }

    private Result(bool isSuccess, T? data, List<Error>? errors = null) {
        IsSuccess = isSuccess;
        Data = data;
        Errors = errors;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(List<Error> errors) => new(false, default, errors);

    internal Result<List<TagResponse>> ToList()
    {
        throw new NotImplementedException();
    }
}