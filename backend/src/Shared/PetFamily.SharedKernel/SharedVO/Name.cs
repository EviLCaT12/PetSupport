using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public class Name : ValueObject
{
    public Name() {}
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

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}