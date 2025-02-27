using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;

public class UpdateVolunteerSocialWebHandler : ICommandHandler<Guid, UpdateVolunteerSocialWebCommand>
{
    private readonly ILogger<UpdateVolunteerSocialWebHandler> _logger;
    private readonly IValidator<UpdateVolunteerSocialWebCommand> _validator;
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerSocialWebHandler(
        ILogger<UpdateVolunteerSocialWebHandler> logger,
        IValidator<UpdateVolunteerSocialWebCommand> validator,
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
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

        var socialWebList = new List<SocialWeb>(socialWebs);
        existedVolunteer.Value.UpdateSocialWebList(socialWebList);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Volunteer`s({id}) social web updated", volunteerId);

        return volunteerId.Value; 
    }
}