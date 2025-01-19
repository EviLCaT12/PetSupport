using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetHandle.ValueObjects;

public record OwnerPhoneNumber
{
    public string Value { get; }

    private OwnerPhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<OwnerPhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<OwnerPhoneNumber>($"PhoneNumber cannot be null or whitespace.");

        var phoneNumber = new OwnerPhoneNumber(value);
        
        return Result.Success(phoneNumber);
    }
}