using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record Color
{
    public string Value { get; }
    
    private Color(string value) => Value = value;

    public static Result<Color, Error> Create(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return Errors.General.ValueIsRequired(nameof(Color));

        var validColor = new Color(color);

        return validColor;
    }
}