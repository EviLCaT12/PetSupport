using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.Shared.SharedVO;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }
    public string Value { get;}

    public static Result<Name, Error.Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(Name));

        if (name.Length > PetConstants.MAX_NAME_LENGTH)
            return Errors.General.LengthIsInvalid(PetConstants.MAX_NAME_LENGTH, nameof(Name)); 
                
        var validName = new Name(name);
        
        return validName;
    }
}