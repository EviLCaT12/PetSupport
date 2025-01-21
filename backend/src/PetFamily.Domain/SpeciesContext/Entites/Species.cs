using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.Entites;

public class Species : Entity<SpeciesId>
{
    public SpeciesId Id { get; private set; }
    
    public Name Name { get; private set; }

    private readonly List<Breed> _breeds;

    public BreedList Breeds
    {
        get
        {
            var petsCreatResult =  BreedList.Create(_breeds);
            return petsCreatResult.Value;
        }
        private set {}
    }
    

    //ef core
    private Species() {}
    private Species(SpeciesId id, Name name, List<Breed> breeds)
    {
        Id = id;
        Name = name;
        _breeds = breeds;
    }

    public static Result<Species> Create(SpeciesId id, Name name, List<Breed> breeds)
    {
        var nameCreateResult = Name.Create(name.Value);
        if (nameCreateResult.IsFailure)
            return Result.Failure<Species>(nameCreateResult.Error);

        var species = new Species(id, nameCreateResult.Value, breeds);
        
        return Result.Success(species); 
    }
}