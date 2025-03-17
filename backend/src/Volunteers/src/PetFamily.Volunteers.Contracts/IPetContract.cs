
using PetFamily.Core.Dto.PetDto;

namespace PetFamily.Volunteers.Contracts;

public interface IPetContract
{
    Task<PetDto?> IsPetHasSpecies(Guid speciesId, CancellationToken cancellationToken);

    Task<PetDto?> IsPetHasBreed(Guid breedId, CancellationToken cancellationToken);
}