using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler : ICommandHandler<Guid, UpdateVolunteerMainInfoCommand>
{
    private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerMainInfoHandler(
        IValidator<UpdateVolunteerMainInfoCommand> validator,
        IVolunteersRepository repository,
        ILogger<UpdateVolunteerMainInfoHandler> logger,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Volunteer`s{id} main info updated", volunteerId);
        
        return volunteerId.Value; 
    }
}