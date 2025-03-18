using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Application;
using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure.Repositories;

public class SpeciesRepository(WriteDbContext context) : ISpeciesRepository
{
    public async Task<Guid> AddAsync(Domain.Entities.Species species, CancellationToken cancellationToken)
    {
        await context.AddAsync(species, cancellationToken);

        return species.Id.Value;
    }
    public async Task<Result<Domain.Entities.Species, ErrorList>> GetByIdAsync(SpeciesId id, CancellationToken cancellationToken)
    {
        var species = await context.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return new ErrorList([Errors.General.ValueNotFound(id.Value)]);

        return species;
    }

    public Guid Remove(Domain.Entities.Species species)
    {
        context.Remove(species);
        
        return species.Id.Value;
    }

    public async Task RemoveBreed(Domain.Entities.Species species, Breed breed, CancellationToken cancellationToken)
    {
        var breedToRemove = species.Breeds.First(b => b.Id == breed.Id);
    }
}