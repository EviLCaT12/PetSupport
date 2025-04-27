using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

public class SoftDeletePetHandler : ICommandHandler<DeletePetCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;
    private readonly IVolunteersRepository _repository;

    public SoftDeletePetHandler(
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetHandler> logger,
        IVolunteersRepository repository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _repository = repository;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(DeletePetCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        var volunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error;
        
        var petId = PetId.Create(command.PetId).Value;
        var pet = volunteer.Value.GetPetById(petId);
        if (pet.IsFailure)
            return pet.Error;
        
        volunteer.Value.SoftDeletePet(pet.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        transaction.Commit();
        return Result.Success<ErrorList>();
    }
}