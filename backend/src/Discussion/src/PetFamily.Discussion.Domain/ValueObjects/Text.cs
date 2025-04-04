using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Domain.ValueObjects;

public class Text : ValueObject
{
    public const int MaxLength = 2000;
    
    private Text() {}

    private Text(string text) => Value = text;
    
    public string Value { get; }

    public static Result<Text, Error> Create(string text)
    {
        if (string.IsNullOrEmpty(text))
            return Errors.General.ValueIsRequired(nameof(text));
        
        if (text.Length > MaxLength)
            return Errors.General.LengthIsInvalid(MaxLength, nameof(text));
        
        return new Text(text);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}