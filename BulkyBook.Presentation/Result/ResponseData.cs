using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBook.Presentation.Result;

public class ResponseData<T>
{
    private readonly T? _value;

    public List<string> ErrorMessages { get; }

    public bool IsSuccess => !IsFailure;

    public bool IsFailure => this.ErrorMessages.Any();

    public T Value
    {
        get
        {
            if (IsSuccess)
            {
                return _value!;
            }

            throw new NullReferenceException("could not access failure result value.");
        }
    }

    internal ResponseData(T? value, List<string> errorMessages)
    {
        _value = value;
        this.ErrorMessages = errorMessages;
    }
}

public class ResponseData
{
    public List<string> ErrorMessages { get; }

    public bool IsSuccess => !IsFailure;

    public bool IsFailure => this.ErrorMessages.Any();

    private ResponseData(List<string> errorMessages)
    {
        this.ErrorMessages = errorMessages;
    }

    public static ResponseData Ok()
    {
        return new ResponseData(new List<string>());
    }

    public static ResponseData Error(string errorMessage)
    {
        return new ResponseData(new List<string> { errorMessage });
    }

    public static ResponseData Error(List<string> errorMessages)
    {
        return new ResponseData(errorMessages);
    }

    public static ResponseData<T> Ok<T>(T value)
    {
        return new ResponseData<T>(value, new List<string>());
    }

    public static ResponseData<T> Error<T>(string errorMessage)
    {
        return new ResponseData<T>(default, new List<string> { errorMessage });
    }

    public static ResponseData<T> Error<T>(List<string> errorMessages)
    {
        return new ResponseData<T>(default, errorMessages);
    }
}