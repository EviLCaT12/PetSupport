using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record AllOwnedPets
{
    private AllOwnedPets(List<Pet> value)
    {
        Value = value;
    }
    
    public IReadOnlyList<Pet> Value { get;}

    public static Result<AllOwnedPets, Error> Create(List<Pet>? pets)
    {
        if (pets == null)
            return ErrorList.General.ValueIsRequired(nameof(AllOwnedPets));

        var validPets = new AllOwnedPets(pets);
        
        return validPets;
    }
    
}