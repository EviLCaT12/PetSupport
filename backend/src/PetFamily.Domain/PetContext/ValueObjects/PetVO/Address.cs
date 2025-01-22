using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record Address
{
    private Address(string city, string street, string houseNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }
    
    public string City { get;}
    
    public string Street { get; }
    
    public string HouseNumber { get; }

    public static Result<Address> Create(string city, string street, string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>("Street cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(houseNumber))
            return Result.Failure<Address>("HouseNumber cannot be null or empty.");
        
        var validAddress = new Address(city,street, houseNumber);
        
        return Result.Success(validAddress);
    }
    
}