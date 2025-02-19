using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    Task<Result<Domain.SpeciesContext.Entities.Species, ErrorList>> GetByIdAsync(
        SpeciesId id,
        CancellationToken cancellationToken);
}