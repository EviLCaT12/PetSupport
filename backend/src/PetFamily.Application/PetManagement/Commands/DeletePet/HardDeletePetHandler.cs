using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Messaging;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Application.PetManagement.Commands.DeletePet;

public class HardDeletePetHandler : ICommandHandler<DeletePetCommand>
{
    private const string BUCKETNAME = "photos";
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;
    private readonly IVolunteersRepository _repository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public HardDeletePetHandler(
        IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetHandler> logger,
        IVolunteersRepository repository,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _repository = repository;
        _messageQueue = messageQueue;
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

            var fileInfos = pet.Value.PhotoList
                .Select(photo => photo.PathToStorage)
                .Select(fileInfo => new FileInfo(fileInfo, BUCKETNAME));
            
            await _messageQueue.WriteAsync(fileInfos, cancellationToken);
            
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