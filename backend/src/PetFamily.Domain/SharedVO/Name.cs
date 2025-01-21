using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Name
{
    private const int MAX_LENGTH = 200;
    private Name(string name)
    {
        Value = name;
    }
    public string Value { get;}

    public static Result<Name> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>("Name cannot be empty");
        
        if(name.Length > MAX_LENGTH)
            return Result.Failure<Name>("Name cannot be longer than " + MAX_LENGTH + " characters");
        
        var validName = new Name(name);
        
        return Result.Success(validName);
    }
}