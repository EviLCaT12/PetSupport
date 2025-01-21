using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.Entites;

public class Species : Entity<SpeciesId>
{
    public SpeciesId Id { get; private set; }
    
    public Name Name { get; private set; }
    

    public BreedList Breeds {get; private set;}
    

    //ef core
    private Species() {}
    private Species(SpeciesId id, Name name, BreedList breeds)
    {
        Id = id;
        Name = name;
        Breeds = breeds;
    }

    public static Result<Species> Create(SpeciesId id, Name name, List<Breed> breeds)
    {
        var nameCreateResult = Name.Create(name.Value);
        if (nameCreateResult.IsFailure)
            return Result.Failure<Species>(nameCreateResult.Error);
        
        var breedListCreateReult  = BreedList.Create(breeds);
        if(breedListCreateReult.IsFailure)
            return Result.Failure<Species>(breedListCreateReult.Error);

        var species = new Species(id, nameCreateResult.Value, breedListCreateReult.Value);
        
        return Result.Success(species); 
    }
}