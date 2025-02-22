using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(ApplicationDbContext context) : ISpeciesRepository
{
    public async Task<Result<Species, ErrorList>> GetByIdAsync(SpeciesId id, CancellationToken cancellationToken)
    {
        var species = await context.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return new ErrorList([Errors.General.ValueNotFound(id.Value)]);

        return species;
    }
    
}