using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Description
{
    private const int MAX_LENGHT = 2000;
    
    private Description(string description)
    {
        Value = description;
    }
    
    public string Value { get;}

    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Description>("Description cannot be empty");
        
        if (description.Length > MAX_LENGHT)
            return Result.Failure<Description>("Description cannot be longer than " + MAX_LENGHT + " symbols");

        var validDescription = new Description(description);
        
        return Result.Success(validDescription);
    }
}