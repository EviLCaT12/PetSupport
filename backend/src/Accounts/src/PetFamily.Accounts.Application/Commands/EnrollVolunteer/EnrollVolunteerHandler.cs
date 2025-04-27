using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;
using PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;

namespace PetFamily.Accounts.Application.Commands.EnrollVolunteer;

public class EnrollVolunteerHandler : ICommandHandler<Guid, EnrollVolunteerCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IAccountManager _accountManager;
    private readonly ILogger<EnrollVolunteerHandler> _logger;
    private readonly IVolunteerContract _contract;
    private readonly IValidator<EnrollVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public EnrollVolunteerHandler(
        UserManager<User> userManager,
        IAccountManager accountManager,
        ILogger<EnrollVolunteerHandler> logger,
        IVolunteerContract contract,
        IValidator<EnrollVolunteerCommand> validator,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _accountManager = accountManager;
        _logger = logger;
        _contract = contract;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(EnrollVolunteerCommand command, 
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var isUserExist = await _userManager.FindByIdAsync(command.UserId.ToString());
        if (isUserExist is null)
            return Errors.General.ValueNotFound().ToErrorList();
        
        var userRole = await _userManager.GetRolesAsync(isUserExist);
        if (userRole.First() != ParticipantAccount.Participant)
            return Errors.General.ValueIsInvalid(
                $"User already has an {userRole.First()} role").ToErrorList();
        
        await _userManager.RemoveFromRoleAsync(isUserExist, ParticipantAccount.Participant);
        
        await _userManager.AddToRoleAsync(isUserExist, VolunteerAccount.Volunteer);

        var participantAccount = await _accountManager.GetParticipantAccountByUserIdAsync(isUserExist.Id);
        if (participantAccount is null)
            return Error
                .Failure("account.not.found", "Has participant user, but not participant account")
                .ToErrorList();
        
        _accountManager.DeleteParticipantAccountAsync(participantAccount);

        var exp = YearsOfExperience.Create(command.Experience).Value;

        var phone = Phone.Create(command.PhoneNumber).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var volunteerAccount = new VolunteerAccount(isUserExist, exp);
        
        await _accountManager.CreateVolunteerAccountAsync(volunteerAccount, cancellationToken);

        var volunteer = await _contract.AddVolunteer(
            new CreateVolunteerRequest(
                new FioDto(isUserExist.FullName.FirstName, isUserExist.FullName.LastName, isUserExist.FullName.Surname),
                phone.Number,
                isUserExist.Email!,
                description.Value),
            cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error;

        volunteerAccount.VolunteerId = volunteer.Value;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return volunteerAccount.Id;

        
    }
    
}