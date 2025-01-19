using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects;

public record Dimensions
{
    public float HeightValue { get; }
    public float WeightValue { get; }

    private Dimensions(float heightValue, float weightValue)
    {
        HeightValue = heightValue;
        WeightValue = weightValue;
    }

    public static Result<Dimensions> Create(float heightValue, float weightValue)
    {
        if (heightValue <= 0)
            return Result.Failure<Dimensions>("Height value must be greater than 0");
        
        if (weightValue <= 0)
            return Result.Failure<Dimensions>("Weight value must be greater than 0");
        
        var dimensions = new Dimensions(heightValue, weightValue);
        
        return Result.Success(dimensions);
    }
}