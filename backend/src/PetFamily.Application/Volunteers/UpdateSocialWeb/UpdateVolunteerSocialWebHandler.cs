using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Validators;
using PetFamily.Application.Validators.Volunteer;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Volunteers.UpdateSocialWeb;

public class UpdateVolunteerSocialWebHandler
{
    private readonly ILogger<UpdateVolunteerSocialWebHandler> _logger;
    private readonly IValidator<UpdateVolunteerSocialWebCommand> _validator;
    private readonly IVolunteersRepository _repository;

    public UpdateVolunteerSocialWebHandler(
        ILogger<UpdateVolunteerSocialWebHandler> logger,
        IValidator<UpdateVolunteerSocialWebCommand> validator,
        IVolunteersRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerSocialWebCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        
        var existedVolunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);

        if (existedVolunteer.IsFailure)
        {
            _logger.LogError("Volunteer with id = {id} not found", volunteerId);
            return existedVolunteer.Error; 
        }
        
        var socialWebs = command.NewSocialWebs
            .Select(d => new {d.Link, d.Name})
            .Select(sw => SocialWeb.Create(sw.Link, sw.Name).Value);
        
        existedVolunteer.Value.UpdateSocialWebList(socialWebs); 
        
        var updateResult = await _repository.UpdateAsync(existedVolunteer.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer`s({id}) social web updated", volunteerId);

        return updateResult; 
    }
}