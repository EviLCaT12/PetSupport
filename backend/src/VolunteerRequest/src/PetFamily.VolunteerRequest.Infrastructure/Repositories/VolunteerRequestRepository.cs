using Microsoft.EntityFrameworkCore;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts.WriteContext;

namespace PetFamily.VolunteerRequest.Infrastructure.Repositories;

public class VolunteerRequestRepository(WriteContext context) : IVolunteerRequestRepository
{
    public async Task<Guid> AddVolunteerRequestAsync(Domain.Entities.VolunteerRequest request,
        CancellationToken cancellationToken)
    {
        await context.VolunteerRequests.AddAsync(request, cancellationToken);
        
        return request.Id.Value;
    }

    public async Task<Domain.Entities.VolunteerRequest?> GetVolunteerRequestByIdAsync(VolunteerRequestId id,
        CancellationToken cancellationToken)
    {
        return await context.VolunteerRequests
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}