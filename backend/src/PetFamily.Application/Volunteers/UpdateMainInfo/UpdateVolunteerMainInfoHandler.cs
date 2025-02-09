using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler
{
    private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;

    public UpdateVolunteerMainInfoHandler(
        IValidator<UpdateVolunteerMainInfoCommand> validator,
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerMainInfoHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerMainInfoCommand updateVolunteerMainInfoCommand,
        CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(updateVolunteerMainInfoCommand, cancellationToken);
        if (result.IsValid == false)
            return result.ToErrorList();
        
        var volunteerId = VolunteerId.Create(updateVolunteerMainInfoCommand.VolunteerId).Value;
        
        var existedVolunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
        if (existedVolunteer.IsFailure)
        {
            _logger.LogError("Volunteer with id = {id} not found", volunteerId);
            return existedVolunteer.Error; 
        }

        var fio = VolunteerFio.Create(
                updateVolunteerMainInfoCommand.Fio.FirstName,
                updateVolunteerMainInfoCommand.Fio.LastName,
                updateVolunteerMainInfoCommand.Fio.SurName)
            .Value;

        var phone = Phone.Create(updateVolunteerMainInfoCommand.Phone).Value;
        
        var email = Email.Create(updateVolunteerMainInfoCommand.Email).Value;
        
        var description = Description.Create(updateVolunteerMainInfoCommand.Description).Value;
        
        var exp = YearsOfExperience.Create(updateVolunteerMainInfoCommand.YearsOfExperience).Value;
        
        existedVolunteer.Value.UpdateMainInfo(
            fio,
            phone,
            email,
            description,
            exp);

        var updateResult =  await _repository.Update(existedVolunteer.Value, cancellationToken); 
        
        return updateResult; 
    }
}