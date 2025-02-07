using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IValidator<UpdateVolunteerCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IValidator<UpdateVolunteerCommand> validator,
        IVolunteersRepository repository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerCommand updateVolunteerCommand,
        CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(updateVolunteerCommand, cancellationToken);
        if (result.IsValid == false)
            return result.ToErrorList();
        
        var volunteerId = VolunteerId.Create(updateVolunteerCommand.VolunteerId); 
        
        var existedVolunteer = await _repository.GetByIdAsync(volunteerId.Value, cancellationToken);
        if (existedVolunteer.IsFailure)
        {
            _logger.LogError("Volunteer with id = {id} not found", volunteerId.Value);
            return existedVolunteer.Error; 
        }
        
        
        return existedVolunteer.Value.Id.Value; 
    }
}