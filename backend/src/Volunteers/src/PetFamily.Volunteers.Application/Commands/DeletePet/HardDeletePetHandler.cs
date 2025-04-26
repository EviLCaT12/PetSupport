using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

public class HardDeletePetHandler : ICommandHandler<DeletePetCommand>
{
    private const string BUCKETNAME = "photos";
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;
    private readonly IVolunteersRepository _repository;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public HardDeletePetHandler(
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
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
}