using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Guid> DeleteAsync(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, ErrorList>> GetByIdAsync(VolunteerId id, CancellationToken cancellationToken = default);

    Task<Guid> UpdateAsync(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Pet, ErrorList>> GetPetByIdAsync(
        VolunteerId id,
        PetId petId,
        CancellationToken cancellationToken);
}