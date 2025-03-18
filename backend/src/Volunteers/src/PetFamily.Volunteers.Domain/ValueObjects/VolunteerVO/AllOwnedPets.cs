using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Domain.Entities;

namespace PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

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
            return Errors.General.ValueIsRequired(nameof(AllOwnedPets));

        var validPets = new AllOwnedPets(pets);
        
        return validPets;
    }
    
}