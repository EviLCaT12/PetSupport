using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler
{
    private const string BUCKET_NAME = "photos";
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(
        ILogger<AddPetPhotosHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotosCommand> validator,
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(AddPetPhotosCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        { 
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
            var getPetResult = getVolunteerResult.Value.AllOwnedPets.FirstOrDefault(p => p.Id == petId);
            if (getPetResult == null)
            {
                 _logger.LogError("Pet with id {petId} not found for volunteer with id {volunteerId}",
                     petId.Value, volunteerId.Value);
                 var error = Errors.General.ValueNotFound(petId.Value);
                 return new ErrorList([error]);
            }
    
            List<PetPhoto> petPhotos = [];
            List<FileData> filesData = [];
            foreach (var photo in command.Photos)
            {
                var extension = Path.GetExtension(photo.FileName);
                var path = Guid.NewGuid();
                var filePath = FilePath.Create(path.ToString(), extension).Value;
    
                var petPhoto = PetPhoto.Create(filePath).Value;
                petPhotos.Add(petPhoto);
                
                var fileData = new FileData(photo.Stream, filePath, BUCKET_NAME);
                filesData.Add(fileData);
            }
            
            var volunteer = getVolunteerResult.Value;
            volunteer.AddPetPhotos(petId, petPhotos);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
    
    
            var uploadPhotosResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
            if (uploadPhotosResult.IsFailure)
                return uploadPhotosResult.Error;
            
            transaction.Commit();
    
            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to add photos to pet {petId}", command.PetId);
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.photo.failure",
                "Error during add photos to pet transaction");

            return new ErrorList([error]);
        }
    }
}