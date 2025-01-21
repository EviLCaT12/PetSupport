using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record PetClassification
{
    public Guid SpeciesId { get;}
    
    public Guid BreedId { get;}

    private PetClassification(Guid speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public static Result<PetClassification> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty)
            return Result.Failure<PetClassification>("Species id cannot be empty");
        
        if (breedId == Guid.Empty)
            return Result.Failure<PetClassification>("Breed id cannot be empty");
        
        var classification = new PetClassification(speciesId, breedId);
        
        return Result.Success(classification);
    }
}