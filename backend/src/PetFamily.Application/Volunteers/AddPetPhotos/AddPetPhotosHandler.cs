using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
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

    public AddPetPhotosHandler(
        ILogger<AddPetPhotosHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotosCommand> validator,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _fileProvider = fileProvider;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(AddPetPhotosCommand command, CancellationToken cancellationToken)
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
        var getPetResult = await _volunteersRepository.GetPetByIdAsync(
            volunteerId,
            petId,
            cancellationToken);
        if (getPetResult.IsFailure)
        {
             _logger.LogError("Failed to get pet with id: {r}", petId);
             var error = Errors.General.ValueNotFound(petId.Value);
             return new ErrorList([error]);
        }

        List<PetPhoto> petPhotos = [];
        List<FileData> filesData = [];
        foreach (var photo in command.Photos)
        {
            var extension = Path.GetExtension(photo.FileName);
            var path = Guid.NewGuid();
            var filePath = FilePath.Create(path, extension).Value;

            var petPhoto = PetPhoto.Create(filePath).Value;
            petPhotos.Add(petPhoto);
            
            var fileData = new FileData(photo.Stream, filePath, BUCKET_NAME);
            filesData.Add(fileData);
        }
        
        var volunteer = getVolunteerResult.Value;
        volunteer.AddPetPhotos(petId, petPhotos);
        await _volunteersRepository.UpdateAsync(volunteer, cancellationToken);


        var uploadPhotosResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
        if (uploadPhotosResult.IsFailure)
            return uploadPhotosResult.Error;

        return Result.Success<ErrorList>();
    }
}