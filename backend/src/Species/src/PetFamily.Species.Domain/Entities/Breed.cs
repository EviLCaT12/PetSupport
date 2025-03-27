using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Domain.ValueObjects.BreedVO;

namespace PetFamily.Species.Domain.Entities;

public class Breed : SoftDeletableEntity<BreedId>
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
        var breed = new Breed(id, name);
        
        return breed;
    }
}