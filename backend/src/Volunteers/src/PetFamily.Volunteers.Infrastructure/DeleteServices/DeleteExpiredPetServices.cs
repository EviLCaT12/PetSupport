using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Infrastructure.DbContexts;

namespace PetFamily.Volunteer.Infrastructure.DeleteServices;

public class DeleteExpiredPetServices
{
    private readonly WriteDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpiredPetServices(WriteDbContext context,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ProcessAsync(int daysBeforeDelete, CancellationToken cancellationToken)
    {
        var volunteerWithPets = await GetVolunteerWithPetsAsync(cancellationToken);

        foreach (var volunteer in volunteerWithPets)
        {
            volunteer.DeleteExpiredPets(daysBeforeDelete);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Volunteers.Domain.Entities.Volunteer>> GetVolunteerWithPetsAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToListAsync(cancellationToken);
    }
}