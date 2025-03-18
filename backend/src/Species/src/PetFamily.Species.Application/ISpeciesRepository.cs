using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application;

public interface ISpeciesRepository
{
    Task<Guid> AddAsync(Domain.Entities.Species species, CancellationToken cancellationToken);
    Task<Result<Domain.Entities.Species, ErrorList>> GetByIdAsync(
        SpeciesId id,
        CancellationToken cancellationToken);
    
    Guid Remove(Domain.Entities.Species species);
}