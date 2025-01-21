using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record Dimensions
{
    public float Height { get; }
    public float Weight { get; }

    private Dimensions(float heightValue, float weightValue)
    {
        Height = heightValue;
        Weight = weightValue;
    }

    public static Result<Dimensions> Create(float height, float weight)
    {
        if (height <= 0)
            return Result.Failure<Dimensions>("Height value must be greater than 0");
        
        if (weight <= 0)
            return Result.Failure<Dimensions>("Weight value must be greater than 0");
        
        var dimensions = new Dimensions(height, weight);
        
        return Result.Success(dimensions);
    }
}