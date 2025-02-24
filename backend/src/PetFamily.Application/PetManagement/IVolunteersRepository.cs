using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement;

public interface IVolunteersRepository
{
    Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, ErrorList>> GetByIdAsync(VolunteerId id, CancellationToken cancellationToken = default);
    
}