using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.SpeciesContext.Entites;

namespace PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;


public record BreedList
{
    private BreedList(List<Breed> breeds)
    {
        Value = breeds;
    }

    public IReadOnlyList<Breed> Value { get; }

    public static Result<BreedList> Create(List<Breed>? breeds)
    {
        if (breeds == null)
            return Result.Failure<BreedList>("Breed list cannot be null");

        var allBreeds = new BreedList(breeds);

        return Result.Success(allBreeds);
    }
}
