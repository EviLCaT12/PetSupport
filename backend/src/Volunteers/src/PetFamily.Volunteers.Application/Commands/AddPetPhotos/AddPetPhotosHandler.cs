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
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(
        ILogger<AddPetPhotosHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotosCommand> validator,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
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

        //FIXME заменить на обращение к файл сервис по готовности
        var photo = Photo.Create(FileId.NewFileId(), "png").Value;
        
        
        var volunteer = getVolunteerResult.Value;
        volunteer.AddPetPhotos(petId, [photo]);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return Result.Success<ErrorList>();
        
    }
}