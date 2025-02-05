using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesContext.Entites;

namespace PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;


public record BreedList
{
    private BreedList(List<Breed> value)
    {
        Value = value;
    }

    public IReadOnlyList<Breed> Value { get; }

    public static Result<BreedList, Error> Create(List<Breed>? breeds)
    {
        if (breeds == null)
            return Errors.General.ValueIsRequired(nameof(BreedList));

        var validBreedList = new BreedList(breeds);

        return validBreedList;
    }
}
