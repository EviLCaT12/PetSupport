using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Providers;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.DeletePetPhotos;

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
      [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork)
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

      List<Photo> photos = [];
      List<FileInfo> fileDatas = [];
      foreach (var photoName in command.PhotoNames)
      {
         var filePath = FilePath.Create(photoName, null).Value;
         
         var petPhoto = Photo.Create(filePath).Value;
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
}