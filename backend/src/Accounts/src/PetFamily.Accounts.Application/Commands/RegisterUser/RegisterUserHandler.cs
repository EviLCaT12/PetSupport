using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountManager accountManager,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork,
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var existedUser = await _userManager.FindByEmailAsync(command.Email);
        if (existedUser != null)
        {
            return Errors.General.AlreadyExist(command.Email).ToErrorList();
        }

        var fio = Fio.Create(command.Fio.FirstName, command.Fio.LastName, command.Fio.SurName);
        if (fio.IsFailure)
        {
            return Errors.General.ValueIsInvalid(fio.Error.Code).ToErrorList();
        }
    
        var role = await _roleManager.FindByNameAsync(ParticipantAccount.Participant);

        var participantUser = User.CreateParticipant(
            command.UserName,
            command.Email,
            fio.Value,
            role!).Value;
    
        var result = await _userManager.CreateAsync(participantUser, command.Password);
        if (result.Succeeded == false)
        {
            _logger.LogError("User creation failed.");
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
        
            return new ErrorList(errors);
        }

        var participantAccount = new ParticipantAccount(participantUser);
    
        await _accountManager.CreateParticipantAccountAsync(participantAccount);
    
        await _userManager.AddToRoleAsync(participantUser, role!.Name!);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
    
        return Result.Success<ErrorList>();
        
    }
}