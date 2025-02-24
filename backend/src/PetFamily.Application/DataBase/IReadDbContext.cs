using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.Application.DataBase;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get; }
    DbSet<PetDto> Pets { get; }
}