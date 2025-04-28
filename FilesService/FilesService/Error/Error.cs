namespace FilesService.Error;

public record Error
{
    public string Code { get;}
    
    public string Message { get; }
    
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Validate(string code, string message) =>
        new(code, message, ErrorType.Validation);
    
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
    
    
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure
}

