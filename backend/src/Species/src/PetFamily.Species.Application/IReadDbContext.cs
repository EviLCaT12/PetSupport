using PetFamily.Core.Dto.BreedDto;
using PetFamily.Core.Dto.SpeciesDto;

namespace PetFamily.Species.Application;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedDto> Breeds { get; }
}