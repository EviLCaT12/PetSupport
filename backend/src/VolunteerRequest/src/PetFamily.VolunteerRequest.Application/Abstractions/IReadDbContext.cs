using PetFamily.VolunteerRequest.Contracts.Dto;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.VolunteerRequest.Application.Abstractions;

public interface IReadDbContext
{
    public IQueryable<VolunteerRequestDto> VolunteerRequests { get; }
}