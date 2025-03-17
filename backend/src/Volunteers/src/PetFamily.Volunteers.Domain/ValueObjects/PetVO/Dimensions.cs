using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetVO;

public record Dimensions
{
    public float Height { get; }
    public float Weight { get; }

    private Dimensions(float height, float weight)
    {
        Height = height;
        Weight = weight;
    }

    public static Result<Dimensions, Error> Create(float height, float weight)
    {
        if (height <= 0)
            return Errors.General.ValueIsInvalid(nameof(Height));
        
        if (weight <= 0)
            return Errors.General.ValueIsInvalid(nameof(Weight));
        
        var validDimensions = new Dimensions(height, weight);

        return validDimensions;
    }
}