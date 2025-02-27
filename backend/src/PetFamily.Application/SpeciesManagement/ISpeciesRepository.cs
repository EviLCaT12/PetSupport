using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.SpeciesManagement;

public interface ISpeciesRepository
{
    Task<Guid> AddAsync(Species species, CancellationToken cancellationToken);
    Task<Result<Species, ErrorList>> GetByIdAsync(
        SpeciesId id,
        CancellationToken cancellationToken);
}