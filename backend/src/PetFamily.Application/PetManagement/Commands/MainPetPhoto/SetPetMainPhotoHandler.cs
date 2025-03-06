using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using FileInfo = PetFamily.Application.Files.FileInfo;


namespace PetFamily.Application.PetManagement.Commands.MainPetPhoto;

public class SetPetMainPhotoHandler : ICommandHandler<PetMainPhotoCommand>
{
    private const string BUCKET_NAME = "photos";
    private readonly ILogger<SetPetMainPhotoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _repository;
    private readonly Providers.IFileProvider _provider;
    private readonly IValidator<PetMainPhotoCommand> _validator;

    public SetPetMainPhotoHandler(
        ILogger<SetPetMainPhotoHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteersRepository repository,
        Providers.IFileProvider provider,
        IValidator<PetMainPhotoCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _provider = provider;
        _validator = validator;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(PetMainPhotoCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
            var volunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error;
            
            var petId = PetId.Create(command.PetId).Value;
            var pet = volunteer.Value.GetPetById(petId);
            if (pet.IsFailure)
                return pet.Error;

            var filePath = FilePath.Create(command.FullPath, null).Value;
            var fileInfo = new FileInfo(filePath, BUCKET_NAME);
            var isPhotoExist = await _provider.GetFilePresignedUrl(fileInfo, cancellationToken);
            if (isPhotoExist.IsFailure)
                return isPhotoExist.Error;

            var petPhoto = volunteer.Value.GetPetPhoto(pet.Value, filePath).Value;

            var setMainPhotoResult = volunteer.Value.SetPetMainPhoto(pet.Value, petPhoto);
            if (setMainPhotoResult.IsFailure)
                return setMainPhotoResult.Error;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Fail to set main pet photo {photo} for pet {pet} for volunteer {volunteerId} in transaction",
                command.FullPath, command.PetId ,command.VolunteerId);
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.failure", "Error during set main pet photo for volunteer transaction");

            return new ErrorList([error]);
        }
    }
}