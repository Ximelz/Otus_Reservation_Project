namespace Admin.Ui.Services;

public sealed record AdminApiResult<T>
{
    public bool Success { get; init; }
    public string? Error { get; init; }
    public T? Data { get; init; }

    public static AdminApiResult<T> Ok(T data) => new() { Success = true, Data = data };
    public static AdminApiResult<T> Fail(string error) => new() { Success = false, Error = error };
}

