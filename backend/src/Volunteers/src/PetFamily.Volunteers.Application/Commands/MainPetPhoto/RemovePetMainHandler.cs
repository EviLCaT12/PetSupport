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


namespace PetFamily.Volunteers.Application.Commands.MainPetPhoto;

public class RemovePetMainPhotoHandler : ICommandHandler<PetMainPhotoCommand>
{
    private const string BUCKET_NAME = "photos";
    private readonly ILogger<SetPetMainPhotoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _repository;
    private readonly IFileProvider _provider;
    private readonly IValidator<PetMainPhotoCommand> _validator;

    public RemovePetMainPhotoHandler(
        ILogger<SetPetMainPhotoHandler> logger,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IVolunteersRepository repository,
        IFileProvider provider,
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

        var removeMainPhotoResult = volunteer.Value.RemovePetMainPhoto(pet.Value, petPhoto);
        if (removeMainPhotoResult.IsFailure)
            return removeMainPhotoResult.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        transaction.Commit();
        return Result.Success<ErrorList>();
    }
}