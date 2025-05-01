using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel.Error;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using FileInfo = PetFamily.Core.Files.FileInfo;

namespace PetFamily.Volunteers.Application.Commands.Delete;

public class HardDeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private const string BUCKETNAME = "photos";
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HardDeleteVolunteerHandler(
        IValidator<DeleteVolunteerCommand> validator,
        IVolunteersRepository repository,
        ILogger<HardDeleteVolunteerHandler> logger,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
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

        
        //FIXME: Удалить фотки животных при удалении волонтера обращением в файлсервису
        _repository.Delete(existedVolunteer.Value, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Volunteer with id = {volunteerId} рфкв deleted" ,volunteerId);
        
        return volunteerId.Value; 
    }
}