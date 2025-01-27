using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

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

    public static Result<Address, Error> Create(string city, string street, string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(city))
            return ErrorList.General.ValueIsRequired(nameof(City));
        
        if (string.IsNullOrWhiteSpace(street))
            return ErrorList.General.ValueIsRequired(nameof(Street));
        
        if (string.IsNullOrWhiteSpace(houseNumber))
            return ErrorList.General.ValueIsRequired(nameof(houseNumber));
        
        var validAddress = new Address(city,street, houseNumber);

        return validAddress;
    }
    
}