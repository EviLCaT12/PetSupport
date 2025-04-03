using Microsoft.EntityFrameworkCore;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Dto.PetDto;

namespace PetFamily.Volunteer.Api.Pets;

public class PetContract : IPetContract
{
    private readonly IReadDbContext _dbContext;

    public PetContract(IReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PetDto?> IsPetHasSpecies(Guid speciesId, CancellationToken cancellationToken)
    {
        var petWithSpecies = await _dbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId, cancellationToken);

        return petWithSpecies;
    }

    public async Task<PetDto?> IsPetHasBreed(Guid breedId, CancellationToken cancellationToken)
    {
        var petWithBreed = await _dbContext.Pets
            .FirstOrDefaultAsync(p => p.BreedId == breedId, cancellationToken);

        return petWithBreed;
    }
}