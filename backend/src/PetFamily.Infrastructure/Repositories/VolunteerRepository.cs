using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository(ApplicationDbContext context) : IVolunteersRepository
{
    public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken); 
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, ErrorList>> GetByIdAsync(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .Include(v => v.AllOwnedPets)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return new ErrorList([Errors.General.ValueNotFound(id.Value)]);

        return volunteer;
    }
    
    public async Task<Guid> Update(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        context.Volunteers.Attach(volunteer);
        
        await context.SaveChangesAsync(cancellationToken); 
        
        return volunteer.Id.Value;
    }
}