#nullable enable
namespace RDF.GL.Common;


//Result pattern for every response of the API

public class Result
{
    protected internal Result(bool isSuccess, bool isWarning, Error error, string message)
    {
           if (isSuccess && !isWarning && error != Error.None)
            {
                throw new InvalidOperationException();
            }
            else if (!isSuccess && !isWarning && error == Error.None)
            {
                throw new InvalidOperationException();
            }
            else
            {
                IsSuccess = isSuccess;
                IsWarning = isWarning;
                Error = error;
                Message = message;
            
            }
    }
    public bool IsSuccess { get; }
    public bool IsWarning { get; set; }
    public string Message { get; set; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, true, Error.None, "");

    public static Result<TValue> Success<TValue>(TValue data) => new(data, true, false, Error.None, "");

    public static Result<TValue> Warning<TValue>(TValue data, string message) => new(data, false, true, Error.None, message);

    public static Result Failure(Error error) => new(false, false, error, "");

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, false, error, "");

    public static Result Create(bool condition) => condition ? Success() : Failure(Error.ConditionNotMet);

    public static Result<TValue> Create<TValue>(TValue? data) => data is not null ? Success(data) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? data, bool isSuccess, bool isWarning, Error error, string message)
        : base(isSuccess, isWarning, error, message) =>
        _value = data;

    public TValue? Value => IsSuccess
        ? _value! : IsWarning ? _value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? data) => Create(data);
}