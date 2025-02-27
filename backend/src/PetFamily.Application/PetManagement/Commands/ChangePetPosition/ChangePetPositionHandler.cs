using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Commands.ChangePetPosition;

public class ChangePetPositionHandler : ICommandHandler<ChangePetPositionCommand>
{
    private readonly ILogger<ChangePetPositionHandler> _logger;
    private readonly IValidator<ChangePetPositionCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetPositionHandler(
        ILogger<ChangePetPositionHandler> logger,
        IValidator<ChangePetPositionCommand> validator,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> HandleAsync(
        ChangePetPositionCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                _logger.LogError("Get error during validation command {commandName}", command.GetType().Name);
                return validationResult.ToErrorList();
            }
            
            var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
            var volunteer = await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error;
            
            var petId = PetId.Create(command.PetId).Value;
            var pet = volunteer.Value.GetPetById(petId);
            if (pet.IsFailure)
            {
                _logger.LogError("Failed to get pet with id: {id}", petId);
                return pet.Error;
            }
            
            var position = Position.Create(command.PetPosition).Value;
            var result = volunteer.Value.MovePetToSpecifiedPosition(petId, position);
            if (result.IsFailure)
            {
                _logger.LogError("Error during change pet ({petId}) position: from {fromPosition} to {toPosition}",
                    petId, pet.Value.Position.Value, command.PetPosition);
                return result.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();

            return Result.Success<ErrorList>();

        }
        catch (Exception e)
        {
            _logger.LogError("Error during transaction of changing pet ({petId}) position to {toPosition}",
                command.PetId, command.PetPosition);
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.position.failure",
                "Error during change pet position transaction");
            return new ErrorList([error]);
        }
    }
}