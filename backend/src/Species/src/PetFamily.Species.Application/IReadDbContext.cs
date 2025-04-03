using PetFamily.Species.Contracts.Dto.BreedDto;
using PetFamily.Species.Contracts.Dto.SpeciesDto;

namespace PetFamily.Species.Application;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedDto> Breeds { get; }
}