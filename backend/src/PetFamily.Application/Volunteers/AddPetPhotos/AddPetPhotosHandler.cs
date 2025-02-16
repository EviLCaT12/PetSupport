using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler
{
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotosCommand> _validator;
    private readonly IFileProvider _fileProvider;

    private AddPetPhotosHandler(
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
        var getPetResult = await _volunteersRepository.GetPetByIdAsync(
            volunteerId,
            petId,
            cancellationToken);
        if (getPetResult.IsFailure)
        {
             _logger.LogError("Failed to get pet with id: {getPetResult.Error}", petId);
             var error = Errors.General.ValueNotFound(petId.Value);
             return new ErrorList([error]);
        }
        
        //Создание фото для сущности и бд
        //Создание объектов данных для файлого хранилища
    }
}