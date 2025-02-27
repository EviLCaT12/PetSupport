using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Application.PetManagement.Commands.DeletePetPhotos;

public class DeletePetPhotosHandler : ICommandHandler<DeletePetPhotosCommand>
{
   private const string BUCKET_NAME = "photos";
   private readonly ILogger<DeletePetPhotosHandler> _logger;
   private readonly IVolunteersRepository _volunteersRepository;
   private readonly IValidator<DeletePetPhotosCommand> _validator;
   private readonly IFileProvider _fileProvider;
   private readonly IUnitOfWork _unitOfWork;

   public DeletePetPhotosHandler(
      ILogger<DeletePetPhotosHandler> logger,
      IVolunteersRepository volunteersRepository,
      IValidator<DeletePetPhotosCommand> validator,
      IFileProvider fileProvider,
      IUnitOfWork unitOfWork)
   {
      _logger = logger;
      _volunteersRepository = volunteersRepository;
      _validator = validator;
      _fileProvider = fileProvider;
      _unitOfWork = unitOfWork;
   }

   public async Task<UnitResult<ErrorList>> HandleAsync(DeletePetPhotosCommand command,
      CancellationToken cancellationToken)
   {
      var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
      try
      {
         var validationResult = await _validator.ValidateAsync(command, cancellationToken);
         if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
      
         var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
         var petId = PetId.Create(command.PetId).Value;
         var volunteer = await _volunteersRepository
            .GetByIdAsync(volunteerId, cancellationToken);
         if (volunteer.IsFailure) 
            if (volunteer.IsFailure)
            {
               _logger.LogError("Failed to get volunteer with id: {id}", volunteerId);
               var error = Errors.General.ValueNotFound(volunteerId.Value);
               return new ErrorList([error]);
            }
         
         var getPetResult = volunteer.Value.GetPetById(petId);
         if (getPetResult.IsFailure)
         {
            _logger.LogError("Failed to get pet with id: {id}", petId);
            return getPetResult.Error;
         }

         List<PetPhoto> photos = [];
         List<FileInfo> fileDatas = [];
         foreach (var photoName in command.PhotoNames)
         {
            var filePath = FilePath.Create(photoName, null).Value;
            
            var petPhoto = PetPhoto.Create(filePath).Value;
            photos.Add(petPhoto);
            
            var fileData = new FileInfo(filePath, BUCKET_NAME);
            fileDatas.Add(fileData);
         }

         var deleteResult = volunteer.Value.DeletePetPhotos(petId, photos);
         if (deleteResult.IsFailure)
            return deleteResult.Error;
         
         await _unitOfWork.SaveChangesAsync(cancellationToken);
         
         var removePhotosResult = await _fileProvider.RemoveFilesAsync(fileDatas, cancellationToken);
         if (removePhotosResult.IsFailure)
            return removePhotosResult.Error;
         
         transaction.Commit();

         return Result.Success<ErrorList>();
      }
      catch (Exception e)
      {
         _logger.LogError(e, "Fail to delete photos for pet {petId}", command.PetId);
         transaction.Rollback();
            
         var error = Error.Failure("volunteer.pet.photo.failure",
            "Error during delete photos for pet transaction");
         return new ErrorList([error]);
      }
   }
}