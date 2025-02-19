using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record Position
{
    public int Value { get;}
    
    private Position(int value) => Value = value;

    public static Result<Position, Error> Create(int value)
    {
        if (value < 1)
            return Errors.General.ValueIsInvalid(nameof(Position));
        

        return new Position(value);
    }

    public Result<Position, Error> Forward(int minNumber ,int maxNumber)
    {
        if (Value == maxNumber)
            return Create(minNumber);
        
        return Create(Value + 1);
    }

    public Result<Position, Error> Backward(int minNumber,int maxNumber)
    {
        if (Value == minNumber)
            return Create(maxNumber);
        
        return Create(Value - 1);
    }
}