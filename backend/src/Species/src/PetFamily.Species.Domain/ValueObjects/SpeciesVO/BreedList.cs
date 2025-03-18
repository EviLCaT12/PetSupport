using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Domain.ValueObjects.SpeciesVO;


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
