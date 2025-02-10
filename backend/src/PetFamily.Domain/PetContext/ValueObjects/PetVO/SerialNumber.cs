using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record SerialNumber
{
    public int Value { get;}
    
    private SerialNumber(int value) => Value = value;

    public static Result<SerialNumber, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid(nameof(SerialNumber));

        return new SerialNumber(value);
    }
}