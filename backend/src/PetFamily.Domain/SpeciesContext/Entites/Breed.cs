using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;

namespace PetFamily.Domain.SpeciesContext.Entites;

public class Breed : Entity<BreedId>
{
    public BreedId Id { get; private set; }
    
    public Name Name { get; private set; }

    //ef core
    private Breed() {}
    private Breed(BreedId id, Name name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Breed, Error> Create(BreedId id, Name name)
    {
        var nameCreateResult = Name.Create(name.Value);
        if (nameCreateResult.IsFailure)
            return nameCreateResult.Error;

        var breed = new Breed(id, nameCreateResult.Value);
        
        return breed;
    }
}