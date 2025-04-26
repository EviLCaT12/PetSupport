using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Application.Commands.BanUserForSendVolunteerRequest;

public class BanUserForSendVolunteerRequestHandler : ICommandHandler<BanUserForSendVolunteerRequestCommand>
{
    private readonly ILogger<BanUserForSendVolunteerRequestHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public BanUserForSendVolunteerRequestHandler(
        ILogger<BanUserForSendVolunteerRequestHandler> logger,
        UserManager<User> userManager,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(BanUserForSendVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var user = await _userManager.Users
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
        {
            _logger.LogError($"User with id {command.UserId} was not found");
            return Errors.General.ValueNotFound(command.UserId).ToErrorList();
        }
        
        user.ParticipantAccount!.BanForSendingRequestUntil = DateTime.UtcNow.AddDays(7);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}