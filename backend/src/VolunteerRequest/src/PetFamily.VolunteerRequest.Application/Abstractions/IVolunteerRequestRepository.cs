using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Abstractions;

public interface IVolunteerRequestRepository
{
    Task<Guid> AddVolunteerRequestAsync(Domain.Entities.VolunteerRequest request,
        CancellationToken cancellationToken);

    Task<Domain.Entities.VolunteerRequest?> GetVolunteerRequestByIdAsync(VolunteerRequestId id,
        CancellationToken cancellationToken);
}