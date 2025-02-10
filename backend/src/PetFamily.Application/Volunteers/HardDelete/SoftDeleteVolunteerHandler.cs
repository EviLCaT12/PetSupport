using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers.HardDelete;

public class SoftDeleteVolunteerHandler
{
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IValidator<DeleteVolunteerCommand> validator,
        IVolunteersRepository repository,
        ILogger<HardDeleteVolunteerHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
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

        existedVolunteer.Value.Delete();
        
        var deleteResult = await _repository.UpdateAsync(existedVolunteer.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer with id = {volunteerId} soft deleted" ,volunteerId);
        
        return deleteResult; 
    }
}