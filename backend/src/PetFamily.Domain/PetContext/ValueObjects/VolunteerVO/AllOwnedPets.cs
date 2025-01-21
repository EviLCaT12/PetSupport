using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record AllOwnedPets
{
    private AllOwnedPets(List<Pet> pets)
    {
        Value = pets;
    }
    
    public IReadOnlyList<Pet> Value { get;}

    public static Result<AllOwnedPets> Create(List<Pet>? pets)
    {
        if (pets == null)
            return Result.Failure<AllOwnedPets>("pets cannot be null");

        var allPets = new AllOwnedPets(pets);
        
        return Result.Success(allPets);
    }
    
}