using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Domain.Shared.SharedVO;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }
    public string Value { get;}

    public static Result<Name> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>("Name cannot be empty");
        
        if(name.Length > PetConstants.MAX_NAME_LENGTH)
            return Result.Failure<Name>("Name cannot be longer than " + PetConstants.MAX_NAME_LENGTH + " characters");
        
        var validName = new Name(name);
        
        return Result.Success(validName);
    }
}