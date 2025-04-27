using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Files;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhotos;

public class AddPetPhotosHandler : ICommandHandler<AddPetPhotosCommand>
{
    private const string BUCKET_NAME = "photos";
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public AddPetPhotosHandler(
        ILogger<AddPetPhotosHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotosCommand> validator,
        IFileProvider fileProvider,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(AddPetPhotosCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
 
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        var petId = PetId.Create(command.PetId).Value;
        var getVolunteerResult =  await _volunteersRepository.GetByIdAsync(volunteerId, cancellationToken);
        if (getVolunteerResult.IsFailure)
        {
            _logger.LogError("Failed to get volunteer with id: {id}", volunteerId);
            var error = Errors.General.ValueNotFound(petId.Value);
            return new ErrorList([error]);
        }

        var getPetResult = getVolunteerResult.Value.GetPetById(petId);
        if (getPetResult.IsFailure)
        {
            _logger.LogError("Failed to get pet with id: {id}", petId);
            return getPetResult.Error;
        }

        List<Photo> petPhotos = [];
        List<FileData> filesData = [];
        foreach (var photo in command.Photos)
        {
            var extension = Path.GetExtension(photo.FileName);
            var path = Guid.NewGuid();
            var filePath = FilePath.Create(path.ToString(), extension).Value;

            var petPhoto = Photo.Create(filePath).Value;
            petPhotos.Add(petPhoto);
            
            var fileInfo = new FileInfo(filePath, BUCKET_NAME);
            var fileData = new FileData(photo.Stream, fileInfo);
            filesData.Add(fileData);
        }
        
        var volunteer = getVolunteerResult.Value;
        volunteer.AddPetPhotos(petId, petPhotos);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        var uploadPhotosResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
        if (uploadPhotosResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);
            return uploadPhotosResult.Error;
        }
            
        
        transaction.Commit();

        return Result.Success<ErrorList>();
        
    }
}