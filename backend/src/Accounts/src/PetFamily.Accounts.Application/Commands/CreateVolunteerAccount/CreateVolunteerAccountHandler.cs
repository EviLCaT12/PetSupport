using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public class CreateVolunteerAccountHandler : ICommandHandler<CreateVolunteerAccountCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountManager _accountManager;
    private readonly ILogger<CreateVolunteerAccountHandler> _logger;

    public CreateVolunteerAccountHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountManager accountManager,
        ILogger<CreateVolunteerAccountHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(CreateVolunteerAccountCommand command, 
        CancellationToken cancellationToken)
    {
        var isUserExists = await _userManager.FindByEmailAsync(command.Email);
        if (isUserExists is not null)
            return Errors.General.AlreadyExist("Email").ToErrorList();

        var expCreateResult = YearsOfExperience.Create(command.Experience);
        if (expCreateResult.IsFailure)
            return expCreateResult.Error.ToErrorList();

        var role = await _roleManager.FindByNameAsync(VolunteerAccount.Volunteer);

        var fio = Fio.Create(command.Fio.FirstName, command.Fio.LastName, command.Fio.SurName);
        if (fio.IsFailure)
        {
            return Errors.General.ValueIsInvalid(fio.Error.Code).ToErrorList();
        }
        
        var volunteerUser = User.CreateVolunteer(
            command.Username,
            command.Email,
            fio.Value,
            role!);
        
        if (volunteerUser.IsFailure)
            return volunteerUser.Error;

        var result = await _userManager.CreateAsync(volunteerUser.Value, command.Password);
        if (result.Succeeded == false)
        {
            _logger.LogError("User creation failed.");
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
            
            return new ErrorList(errors);
        }

        var volunteerAccount = new VolunteerAccount(volunteerUser.Value, expCreateResult.Value);
        
        await _accountManager.CreateVolunteerAccountAsync(volunteerAccount);
        
        await _userManager.AddToRoleAsync(volunteerUser.Value, role!.Name!);

        return Result.Success<ErrorList>();
    }
}