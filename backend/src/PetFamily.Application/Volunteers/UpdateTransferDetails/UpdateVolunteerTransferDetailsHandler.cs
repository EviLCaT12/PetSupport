using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.UpdateTransferDetails;

public class UpdateVolunteerTransferDetailsHandler
{
    private readonly ILogger<UpdateVolunteerTransferDetailsHandler> _logger;
    private readonly IValidator<UpdateVolunteerTransferDetailsCommand> _validator;
    private readonly IVolunteersRepository _repository;


    public UpdateVolunteerTransferDetailsHandler(
        ILogger<UpdateVolunteerTransferDetailsHandler> logger,
        IValidator<UpdateVolunteerTransferDetailsCommand> validator,
        IVolunteersRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateVolunteerTransferDetailsCommand command,
        CancellationToken cancellationToken = default)
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
        
        var transferDetails = command.NewTransferDetails
            .Select(d => new {d.Name, d.Description})
            .Select(td => TransferDetails.Create(td.Name, td.Name).Value);


        var transferDetailsLit = new ValueObjectList<TransferDetails>(transferDetails);
        
        existedVolunteer.Value.UpdateTransferDetailsList(transferDetailsLit); 
        
        var updateResult = await _repository.UpdateAsync(existedVolunteer.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer`s({id}) transfer details updated", volunteerId);

        return updateResult; 
    }
}