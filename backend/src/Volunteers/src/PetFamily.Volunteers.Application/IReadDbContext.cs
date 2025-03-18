using PetFamily.Core.Dto.PetDto;
using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Application;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}