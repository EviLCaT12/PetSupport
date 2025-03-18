using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Contracts.Requests.Species;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<UnitResult<ErrorList>> IsSpeciesHasBreed(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken);

    Task<Result<Guid, ErrorList>> AddSpecies(CreateRequest request, CancellationToken cancellationToken);

    Task<Result<Domain.Entities.Species, ErrorList>> GetSpeciesById(Guid speciesId, CancellationToken cancellationToken);

    Task<Result<List<Guid>, ErrorList>> AddBreeds(
        Guid speciesId,
        AddBreedsRequest request,
        CancellationToken cancellationToken);
}