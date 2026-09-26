namespace Telecube.BuildingBlocks.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() =>  new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess? _value! : throw new InvalidOperationException();
    protected Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    public static Result<T> Success(T value) => new(value, true, Error.None);
    public static new Result<T> Failure(Error error) => new(default!, false, error);
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) => IsSuccess? onSuccess(Value) : onFailure(Error);
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> next) => IsSuccess ? next(Value) : Result<TOut>.Failure(Error);
    public Result<TOut> Map<TOut>(Func<T, TOut> selector) => IsSuccess ? Result<TOut>.Success(selector(Value)) : Result<TOut>.Failure(Error);
}