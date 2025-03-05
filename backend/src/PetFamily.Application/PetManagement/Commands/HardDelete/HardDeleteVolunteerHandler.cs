using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Application.PetManagement.Commands.HardDelete;

public class HardDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private const string BUCKETNAME = "photos";
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public HardDeleteVolunteerHandler(
        IValidator<DeleteVolunteerCommand> validator,
        IVolunteersRepository repository,
        ILogger<HardDeleteVolunteerHandler> logger,
        IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        
        var existedVolunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
        if (existedVolunteer.IsFailure)
        {
            _logger.LogError("Volunteer with id = {id} not found", volunteerId);
            return existedVolunteer.Error; 
        }

        _repository.Delete(existedVolunteer.Value, cancellationToken);

        var allPets = existedVolunteer.Value.AllOwnedPets;
        List<FileInfo> fileInfos = [];
        foreach (var pet in allPets)
        {
            fileInfos.AddRange(pet.PhotoList
                .Select(photo => new FileInfo(photo.PathToStorage, BUCKETNAME)));
        }
        
        await _messageQueue.WriteAsync(fileInfos, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Volunteer with id = {volunteerId} рфкв deleted" ,volunteerId);
        
        return volunteerId.Value; 
    }
}