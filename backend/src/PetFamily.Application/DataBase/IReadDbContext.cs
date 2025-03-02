using PetFamily.Application.Dto.BreedDto;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.SpeciesDto;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.DataBase;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedDto> Breeds { get; }
}