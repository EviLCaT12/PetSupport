using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Entitues;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var existedUser = await _userManager.FindByEmailAsync(command.Email);
        if (existedUser != null)
        {
            return Errors.General.AlreadyExist(command.Email).ToErrorList();
        }

        // var fio = Fio.Create(command.fio.FirstName, command.fio.LastName, command.fio.SurName);
        // if (fio.IsFailure)
        // {
        //     return Errors.General.ValueIsInvalid(fio.Error.Code).ToErrorList();
        // }
        
        var role = await _roleManager.FindByNameAsync("Default");

        var user = User.CreateParticipant(
            command.UserName,
            command.Email,
            role).Value;
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded == false)
        {
            _logger.LogError("User creation failed.");
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
            
            return new ErrorList(errors);
        }
        
        var res = await _userManager.AddToRoleAsync(user, "Default");
        
        return Result.Success<ErrorList>();
    }
}