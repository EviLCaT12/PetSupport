using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.DataBase;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}