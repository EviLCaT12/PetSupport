using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Commands.DeletePet;

public class HardDeletePetHandler : ICommandHandler<DeletePetCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;
    private readonly IVolunteersRepository _repository;

    public HardDeletePetHandler(
        IUnitOfWork unitOfWork,
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
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
            var volunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error;
            
            var petId = PetId.Create(command.PetId).Value;
            var pet = volunteer.Value.GetPetById(petId);
            if (pet.IsFailure)
                return pet.Error;
            
            volunteer.Value.HardDeletePet(pet.Value);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Fail to soft delete pet {petId} for volunteer {volunteerId} in transaction", command.PetId ,command.VolunteerId);
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.failure", "Error during soft delete pet for volunteer transaction");

            return new ErrorList([error]);
        }
    }
}