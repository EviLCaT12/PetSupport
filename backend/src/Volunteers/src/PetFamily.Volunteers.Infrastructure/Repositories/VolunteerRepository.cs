using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteer.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteer.Infrastructure.Repositories;

public class VolunteerRepository(WriteDbContext context) : IVolunteersRepository
{
    public async Task<Guid> AddAsync(Volunteers.Domain.Entities.Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        
        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteers.Domain.Entities.Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        context.Volunteers.Remove(volunteer);
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteers.Domain.Entities.Volunteer, ErrorList>> GetByIdAsync(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .Include(v => v.AllOwnedPets)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return new ErrorList([Errors.General.ValueNotFound(id.Value)]);

        return volunteer;
    }
    
}