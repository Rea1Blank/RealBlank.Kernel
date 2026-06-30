namespace RealBlank.Kernel.Result;

public class Result<T, E>
{
    private readonly T? _value;
    private readonly E? _error;
    private readonly bool _isSuccess;

    public T? Value => IsSuccess ? _value : throw new ResultException(_error?.ToString() ?? "unkown");

    public E? Error { get; }

    public bool IsSuccess => _isSuccess;

    public bool IsFailure => !IsFailure;

    private Result(T? value, E? error)
    {
        if (value is not null && error is not null)
            throw new InvalidOperationException("Result cannot have both value and error");

        _error = error;
        _value = value;
        _isSuccess = error is null;
    }


    public static Result<T, E> Ok(T value) => new(value, default);
    public static Result<T, E> Err(E value) => new(default, value);

    public TResult Match<TResult>(Func<T, TResult> onOk, Func<E, TResult> onErr) => IsSuccess ? onOk(_value!) : onErr(_error!);
    public Result<TResult, E> Map<TResult>(Func<T, TResult> mapper) => IsSuccess ? Result<TResult, E>.Ok(mapper(_value!)) : Result<TResult, E>.Err(_error!);
    public Result<TResult, E> Bind<TResult>(Func<T, Result<TResult, E>> binder) => IsSuccess ? binder(_value!) : Result<TResult, E>.Err(_error!);
}

