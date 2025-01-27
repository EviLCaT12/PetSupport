using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

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

    public static Result<PetClassification, Error> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty)
            return ErrorList.General.ValueIsRequired(nameof(SpeciesId));
        
        if (breedId == Guid.Empty)
            return ErrorList.General.ValueIsRequired(nameof(BreedId));
        
        var validClassification = new PetClassification(speciesId, breedId);

        return validClassification;
    }
}